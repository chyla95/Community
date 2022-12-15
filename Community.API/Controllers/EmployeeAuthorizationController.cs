using AutoMapper;
using Community.API.Dtos.Employee.Authorization;
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
    public class EmployeeAuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService<Employee> _userService;
        private readonly ISettingsAccessor _appSettings;

        public EmployeeAuthorizationController(IMapper mapper, IUserService<Employee> userService, ISettingsAccessor appSettings)
        {
            _mapper = mapper;
            _userService = userService;
            _appSettings = appSettings;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<EmployeeAuthorizationResponseDto>> SignUp(EmployeeSignUpRequestDto employeeSignUpRequestDto)
        {
            bool isEmailTaken = await _userService.IsEmailTaken(employeeSignUpRequestDto.Email);
            if (isEmailTaken) throw new HttpBadRequestException("Email adress is taken!");

            Employee employee = _mapper.Map<Employee>(employeeSignUpRequestDto);
            await _userService.AddAsync(employee);

            EmployeeAuthorizationResponseDto response = _mapper.Map<EmployeeAuthorizationResponseDto>(employee);
            string JwtTokenSecret = _appSettings.GetValue(Constants.JWT_SECRET_KEY);
            response.JwtToken = employee.CreateJwtToken(JwtTokenSecret);

            return Ok(response);
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<EmployeeAuthorizationResponseDto>> SignIn(EmployeeSignInRequestDto employeeSignInRequestDto)
        {
            Employee? employee = await _userService.GetAsync(employeeSignInRequestDto.Email);
            if (employee == null) throw new HttpBadRequestException("Invalid credentials!");
            if (!employee.ComparePassword(employeeSignInRequestDto.Password)) throw new HttpBadRequestException("Invalid credentials!");

            EmployeeAuthorizationResponseDto response = _mapper.Map<EmployeeAuthorizationResponseDto>(employee);
            string JwtTokenSecret = _appSettings.GetValue(Constants.JWT_SECRET_KEY);
            response.JwtToken = employee.CreateJwtToken(JwtTokenSecret);

            return Ok(response);
        }

    }
}
