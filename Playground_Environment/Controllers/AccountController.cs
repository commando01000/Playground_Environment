using Common.Layer;
using Services.Layer.Dtos;
using Services.Layer.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace Playground_Environment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto input)
        {
            var user = await _userService.Login(input);
            if (user is not null)
            {
                return user;
            }
            else
            {
                return BadRequest("User not found");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto input)
        {
            var user = await _userService.Register(input);
            if (user is not null)
            {
                return user;
            }
            else
            {
                return BadRequest("User already exists");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUser();
            if (user is not null)
            {
                return Ok(user);
            }
            else
            {
                return Ok(new { status = false, message = "User not found", data = new UserDto() });
            }
        }
    }
}
