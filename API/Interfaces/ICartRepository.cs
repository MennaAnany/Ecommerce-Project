using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ICartRepository
    {
        void AddCart(Cart cart);
        Task<Cart> GetCartAsync(int userId);
        Task<CartDto> GetUserCartAsync(int userId);
        void DeleteCart(Cart cart);
        void DeleteCartItem(Cart cart, CartItem item);
        CartItem GetCartItems(Cart cart, AddCartItemDto itemDto);
        CartItem GetCartItem(Cart cart, int id);
        void UpdateCart(Cart cart);
    }
}
