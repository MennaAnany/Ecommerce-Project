using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserDto> AddUser(string username, string email, string password)
        {
            var user = new AppUser
            {
                UserName = username,
                Email = email,
            };

            _context.Users.Add(user);


            if (await _context.SaveChangesAsync() < 0) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            };
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {

            var users = await _context.Users.ToListAsync();

            var userDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return userDto;

        }
    }
}