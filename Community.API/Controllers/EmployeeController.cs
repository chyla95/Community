using AutoMapper;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetMany()
        {
            IEnumerable<Employee> employees = await _employeeService.GetManyAsync();

            IEnumerable<EmployeeResponseDto> response = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeResponseDto>> Get(int id)
        {
            Employee? employee = await _employeeService.GetAsync(id);
            if (employee == null) throw new HttpNotFoundException("Employee not found!");

            EmployeeResponseDto response = _mapper.Map<EmployeeResponseDto>(employee);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [EmployeeAuthorization(Permission.ManageEmployees)]
        public async Task<ActionResult> Update(int id, EmployeeRequestDto employeeRequestDto)
        {
            Employee? employee = await _employeeService.GetAsync(id);
            if (employee == null) throw new HttpNotFoundException("Employee not found!");

            bool isEmailTaken = await _employeeService.IsEmailTakenAsync(employeeRequestDto.Email, id);
            if (isEmailTaken) throw new HttpBadRequestException("Email is taken!");

            _mapper.Map(employeeRequestDto, employee);
            await _employeeService.UpdateAsync(employee);

            return Ok();
        }
    }
}
