using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Services.Layer.Dtos;

namespace Services.Layer.Services.User
{
    public interface IUserService
    {
        public Task<UserDto> Login(LoginDto loginDto);
        public Task<UserDto> Register(RegisterDto registerDto);
        public Task<UserDto> GetCurrentUser(); // get current user
    }
}
