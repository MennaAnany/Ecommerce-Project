using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        public List<string> Colors { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
