using Community.API.Filters;
using Community.API.Utilities.Accessors;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService<Staff> _userService;
        private readonly IContextAccessor _currentUser;

        public TestController(IUserService<Staff> userService,IContextAccessor currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [HttpGet("1")]
        public ActionResult Test1()
        {
            return Ok(_currentUser.GetUser<Staff>());
        }
        [AllowAnonymous]
        [HttpGet("2")]
        [Authorization(Name = "Test")]
        public async Task<ActionResult> Test2()
        {
            return Ok(await _userService.GetManyAsync());
        }
    }
}
