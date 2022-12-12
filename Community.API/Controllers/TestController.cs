using Community.API.Utilities.Accessors;
using Community.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IContextAccessor _currentUser;

        public TestController(IContextAccessor currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        public ActionResult Test()
        {
            return Ok(_currentUser.GetUser<Staff>());
        }
    }
}
