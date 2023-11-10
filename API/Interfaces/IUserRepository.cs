using API.DTOs;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<UserDto> AddUser(string email, string username, string password);
    }
}
