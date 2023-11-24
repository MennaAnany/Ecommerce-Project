using API.Entities;

namespace API.DTOs
{
    public class CartDto
    {
        public decimal SubTotal { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
