using Data.Layer.Entities.Identity;
using Services.Layer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Services.Token
{
    public interface ITokenService
    {
        public Task<string> GenerateAccessToken(AppUser user);
        public string GenerateRefreshToken();
    }
}
