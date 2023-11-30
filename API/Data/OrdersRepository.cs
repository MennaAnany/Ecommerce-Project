using API.DTOs;
using API.Entities;
using API.Entities.API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly DataContext _context;
        public OrdersRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CheckoutAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);

            if (cart == null || cart.CartItems.Count == 0) return null;

            var order = new Order
            {
                AppUserId = userId,
                OrderItems = cart.CartItems.Select(c => new OrderItems
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Color = c.Color,
                    Size = c.Size
                }).ToList(),
                SubTotal = cart.SubTotal
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear cart
            cart.CartItems.Clear();
            cart.SubTotal = 0;

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return new OrderDto 
            {
                Id = order.Id,
                AppUserId = order.AppUserId,
                CreatedAt = order.CreatedAt,
                SubTotal = order.SubTotal
              
            };
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.AppUserId == userId)
                .Include(c => c.OrderItems)
                .ThenInclude(p => p.Product)
                .ToListAsync();
        }

    }
}

