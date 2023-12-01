using API.Entities;
using System.Security.Claims;

namespace API.Interfaces
{
    public interface ITokenService
    {
        Task<ClaimsIdentity> CreateToken(AppUser user);
    }
}
