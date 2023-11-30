using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IOrdersRepository
    {
        Task<OrderDto> CheckoutAsync(int userId);
        Task<List<Order>> GetOrdersAsync(int userId);
    }
}
