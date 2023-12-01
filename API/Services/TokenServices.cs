using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        public TokenService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsIdentity> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
      {
          new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
          new Claim(ClaimTypes.Name,user.UserName.ToString()),
      };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return new ClaimsIdentity(
              claims, CookieAuthenticationDefaults.AuthenticationScheme);

        }
    }
}