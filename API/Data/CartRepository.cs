using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _maper;
        public CartRepository(DataContext context, IMapper maper)
        {
            _maper = maper;
            _context = context;
        }

        public void AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
        }
        public void DeleteCart(Cart cart)
        {
            _context.Carts.Remove(cart);
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            return await _context.Carts.Where(c => c.AppUserId == userId)
            .Include(c => c.CartItems)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync();
        }
        public async Task<CartDto> GetUserCartAsync(int userId)
        {
            return await _context.Carts.Where(c => c.AppUserId == userId)
            .Include(c => c.CartItems)
            .ProjectTo<CartDto>(_maper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }
        public CartItem GetCartItems(Cart cart, AddCartItemDto itemDto)
        {
            return cart.CartItems.FirstOrDefault(c =>
                c.CartId == cart.Id
                && c.Color == itemDto.Color
                && c.Size == itemDto.Size
                && c.ProductId == itemDto.ProductId);
        }

        public CartItem GetCartItem(Cart cart, int id)
        {
            return cart.CartItems.FirstOrDefault(c => c.ProductId == id);
        }

        public void DeleteCartItem(Cart cart, CartItem item)
        {
            cart.CartItems.Remove(item);
        }

        public async void UpdateCart(Cart cart)
        {
            cart.SubTotal = cart.CartItems
                .Sum(item =>
                {
                    var product = _context.Products.Find(item.ProductId);
                    return product?.Price * item.Quantity ?? 0;
                });

            _context.Entry(cart).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
