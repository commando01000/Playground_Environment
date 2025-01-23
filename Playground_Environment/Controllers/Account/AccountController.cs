using Common.Layer;
using Services.Layer.Dtos;
using Services.Layer.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Data.Layer.Entities.Identity;

namespace Playground_Environment.Controllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserService userService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto input)
        {
            if (input is not null)
            {
                var user = await _userService.Login(input);
                if (user is not null)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest("User not found");
                }
            }
            else
            {
                return BadRequest("Invalid input");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest($"Error from external provider: {remoteError}");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("Error loading external login information.");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var profilePicture = info.Principal.FindFirstValue("picture"); // Google provides a profile picture URL
                // If the user does not exist, create a new user and link the external login
                user = new AppUser
                {
                    UserName = name,
                    Email = email,
                    DisplayName = name,
                    EmailConfirmed = true // Mark email as confirmed since it's from Google
                };

                var createUserResult = await _userManager.CreateAsync(user);
                if (createUserResult.Succeeded)
                {
                    // Link the external login to the new user
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (addLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl ?? "/");
                    }
                    else
                    {
                        return BadRequest("Failed to link external login to the new user.");
                    }
                }
                else
                {
                    return BadRequest("Failed to create a new user.");
                }
            }
        }

        [AllowAnonymous]
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
