namespace API.DTOs
{
    public class UpdateProductDto
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
