using AutoMapper;
using Community.API.Dtos.Customer;
using Community.API.Dtos.Employee;
using Community.API.Filters;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using static Community.Domain.Models.Employee;

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;

        public CustomerController(IMapper mapper, ICustomerService customerService)
        {
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetMany()
        {
            IEnumerable<Customer> customers = await _customerService.GetManyAsync();

            IEnumerable<CustomerResponseDto> response = _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerResponseDto>> Get(int id)
        {
            Customer? customer = await _customerService.GetAsync(id);
            if (customer == null) throw new HttpNotFoundException("Customer not found!");

            CustomerResponseDto response = _mapper.Map<CustomerResponseDto>(customer);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [EmployeeAuthorization(Permission.CanManageCustomers)]
        public async Task<ActionResult> Update(int id, CustomerRequestDto customerRequestDto)
        {
            Customer? customer = await _customerService.GetAsync(id);
            if (customer == null) throw new HttpNotFoundException("Customer not found!");

            bool isEmailTaken = await _customerService.IsEmailTakenAsync(customerRequestDto.Email, id);
            if (isEmailTaken) throw new HttpBadRequestException("Email is taken!");

            _mapper.Map(customerRequestDto, customer);
            await _customerService.UpdateAsync(customer);

            return Ok();
        }

        [HttpPost("ToEmployee/{id:int}")]
        [EmployeeAuthorization(Permission.CanManageEmployees)]
        public async Task<ActionResult> ToEmployee(int id)
        {
            Customer? customer = await _customerService.GetAsync(id);
            if (customer == null) throw new HttpNotFoundException("Customer not found!");

            Employee employee = _mapper.Map<Employee>(customer);
            await _customerService.ConvertToEmployeeAsync(customer, employee);

            return Ok();
        }
    }
}
