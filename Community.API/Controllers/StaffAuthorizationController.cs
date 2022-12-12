using AutoMapper;
using Community.API.Dtos.Staff.Authorization;
using Community.API.Utilities;
using Community.API.Utilities.Accessors;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffAuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService<Staff> _userService;
        private readonly ISettingsAccessor _appSettings;

        public StaffAuthorizationController(IMapper mapper, IUserService<Staff> userService, ISettingsAccessor appSettings)
        {
            _mapper = mapper;
            _userService = userService;
            _appSettings = appSettings;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<StaffAuthorizationResponseDto>> SignUp(StaffSignUpRequestDto staffSignUpRequestDto)
        {
            bool isEmailTaken = await _userService.IsEmailTaken(staffSignUpRequestDto.Email);
            if (isEmailTaken) throw new HttpBadRequestException("Email adress is taken!");

            Staff staff = _mapper.Map<Staff>(staffSignUpRequestDto);
            await _userService.AddAsync(staff);

            StaffAuthorizationResponseDto response = _mapper.Map<StaffAuthorizationResponseDto>(staff);
            string JwtTokenSecret = _appSettings.GetValue(Configuration.JWT_SECRET_KEY);
            response.JwtToken = staff.CreateJwtToken(JwtTokenSecret);

            return Ok(response);
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<StaffAuthorizationResponseDto>> SignIn(StaffSignInRequestDto staffSignInRequestDto)
        {
            Staff? staff = await _userService.GetAsync(staffSignInRequestDto.Email);
            if (staff == null) throw new HttpBadRequestException("Invalid credentials!");
            if (!staff.ComparePassword(staffSignInRequestDto.Password)) throw new HttpBadRequestException("Invalid credentials!");

            StaffAuthorizationResponseDto response = _mapper.Map<StaffAuthorizationResponseDto>(staff);
            string JwtTokenSecret = _appSettings.GetValue(Configuration.JWT_SECRET_KEY);
            response.JwtToken = staff.CreateJwtToken(JwtTokenSecret);

            return Ok(response);
        }

    }
}
