namespace API.DTOs
{
    public class CartDto
    {
        public decimal SubTotal { get; set; }
        public ICollection<CartItemDto> Items { get; set; }
    }
}
