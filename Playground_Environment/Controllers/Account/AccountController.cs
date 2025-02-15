using Common.Layer;
using Services.Layer.Dtos;
using Services.Layer.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Data.Layer.Entities.Identity;
using Services.Layer.Services.Token;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Playground_Environment.Controllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(IUserService userService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
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
        public async Task<ActionResult> ExternalLogin(string provider, string returnUrl)
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

            AppUser user;
            if (result.Succeeded)
            {
                // User already exists, retrieve it
                user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = name,
                        Email = email,
                        DisplayName = name,
                        EmailConfirmed = true
                    };

                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        return BadRequest("Failed to create a new user.");
                    }

                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        return BadRequest("Failed to link external login.");
                    }
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            // Generate JWT Token
            var token = await _tokenService.GenerateAccessToken(user);

            // Return JSON response instead of redirecting
            return Redirect($"http://localhost:4200/external-login-callback?token={token}");
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
