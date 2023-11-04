using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]  
        public string Description { get; set; }
        [Required]  
        public List<string> Images { get; set; }
        [Required] 
        public List<string> Colors { get; set; }
        [Required] 
        public string Category { get; set; }
    }
}
 