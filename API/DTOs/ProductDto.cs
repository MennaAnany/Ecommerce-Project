namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<string> Images { get; set; }
        public List<string> Colors { get; set; }
        public string Category { get; set; }

    }
}

