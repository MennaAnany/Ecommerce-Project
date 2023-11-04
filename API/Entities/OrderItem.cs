using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{

    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
