using AutoMapper;
using Community.API.Dtos.Authentication;
using Community.API.Utilities;
using Community.API.Utilities.Accessors;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly ISettingsAccessor _appSettings;

        public AuthorizationController(IMapper mapper, ICustomerService customerService, ISettingsAccessor appSettings)
        {
            _mapper = mapper;
            _customerService = customerService;
            _appSettings = appSettings;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<CustomerAuthenticationResponseDto>> SignUp(CustomerSignUpRequestDto customerSignUpRequestDto)
        {
            bool isEmailTaken = await _customerService.IsEmailTakenAsync(customerSignUpRequestDto.Email);
            if (isEmailTaken) throw new HttpBadRequestException("Email adress is taken!");

            Customer customer = _mapper.Map<Customer>(customerSignUpRequestDto);
            await _customerService.AddAsync(customer);

            CustomerAuthenticationResponseDto response = _mapper.Map<CustomerAuthenticationResponseDto>(customer);
            string jwtSecret = _appSettings.GetValue(Constants.AppSettingsKeys.JWT_SECRET);
            response.Jwt = customer.CreateJwt(jwtSecret);

            return Ok(response);
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<CustomerAuthenticationResponseDto>> SignIn(CustomerSignInRequestDto customerSignInRequestDto)
        {
            Customer? customer = await _customerService.GetByEmailAsync(customerSignInRequestDto.Email);
            if (customer == null) throw new HttpBadRequestException("Invalid credentials!");
            if (!customer.ComparePassword(customerSignInRequestDto.Password)) throw new HttpBadRequestException("Invalid credentials!");

            CustomerAuthenticationResponseDto response = _mapper.Map<CustomerAuthenticationResponseDto>(customer);
            string jwtSecret = _appSettings.GetValue(Constants.AppSettingsKeys.JWT_SECRET);
            response.Jwt = customer.CreateJwt(jwtSecret);

            return Ok(response);
        }
    }
}
