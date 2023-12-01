using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [Authorize]
        [HttpPost("checkout")]
        public async Task<ActionResult<OrderDto>> Checkout()
        {
            var userId = User.GetUserId();
            var orderDto = await _unitOfWork.OrdersRepository.CheckoutAsync(userId);

            if (orderDto == null) return NotFound("Cart is empty, Add items to your cart");

            return Ok(orderDto);
        }

        [Authorize]
        [HttpGet("my-orders")]
        public async Task<ActionResult<OrderDto>> GetMyOrders()
        {
            var userId = User.GetUserId();
            var orders = await _unitOfWork.OrdersRepository.GetOrdersAsync(userId);

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return Ok(orderDtos);
        }

        [Authorize]
        [HttpDelete("delete-order/{orderId}")]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var userId = User.GetUserId();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.AppUserId == userId);

            if (order == null)
                return NotFound("Order not found for this user");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
