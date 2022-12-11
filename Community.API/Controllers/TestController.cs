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
        private readonly ICurrentUser<Staff> _currentUser;

        public TestController(ICurrentUser<Staff> currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        public ActionResult Test()
        {
            return Ok(_currentUser.GetUser());
        }
    }
}
