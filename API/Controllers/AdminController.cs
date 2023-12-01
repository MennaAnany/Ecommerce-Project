using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _context;


        public AdminController(DataContext context, UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-users")]
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return userDto;

        }

        [Authorize(Roles = "Admin")] 
        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound("User not found");

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            };

            return userDto;
        }

        [Authorize(Roles = "Admin")] 
        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? NoContent() : BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getStatistics")]
        public async Task<ActionResult<IEnumerable<UserStatisticsDto>>> GetStatistics()
        {
            var users = await _userManager.Users.ToListAsync();
            var userStatistics = new List<UserStatisticsDto>();

            foreach (var user in users)
            {
                // Exclude admin from statistics
                if (await _userManager.IsInRoleAsync(user, "Admin")) continue;

                var orders = await _unitOfWork.OrdersRepository.GetOrdersAsync(user.Id);
                var totalOrders = orders.Count;
                var mostBoughtProduct = await _unitOfWork.ProductRepository.FindMostBoughtProduct(orders);

                var statisticsDto = new UserStatisticsDto
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    TotalOrders = totalOrders,
                    MostBoughtProduct = new StatisticsDto
                    {
                        Id = mostBoughtProduct.Id,
                        Name = mostBoughtProduct.Name,
                        Slug = mostBoughtProduct.Slug,
                        Price = mostBoughtProduct.Price,
                        Quantity = mostBoughtProduct.Quantity,
                      
                    }
                };
                userStatistics.Add(statisticsDto);
            }
            // Order users by the number of their total orders
            userStatistics = userStatistics.OrderByDescending(u => u.TotalOrders).ToList();

            return Ok(userStatistics);
        }

    }
}
