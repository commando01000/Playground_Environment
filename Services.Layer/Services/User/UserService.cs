using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Layer.Dtos;
using Services.Layer.Services.Token;

namespace Services.Layer.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<AppUser> userManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _loggerFactory = loggerFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserDto> GetCurrentUser()
        {
            // get current logged in user
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
            };
        }
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    _loggerFactory.CreateLogger<UserService>().LogError("User not found");
                    return null;
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (result)
                {
                    return new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = await _tokenService.GenerateAccessToken(user)
                    };
                }
                else
                {
                    _loggerFactory.CreateLogger<UserService>().LogError("Invalid password");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger<UserService>().LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(registerDto.Email);

                if (user != null)
                {
                    _loggerFactory.CreateLogger<UserService>().LogError("User already exists");
                }

                var appUser = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                };

                var result = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }
                else
                {
                    return new UserDto
                    {
                        Id = appUser.Id,
                        Email = appUser.Email,
                        DisplayName = appUser.DisplayName,
                        Token = await _tokenService.GenerateAccessToken(appUser)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
