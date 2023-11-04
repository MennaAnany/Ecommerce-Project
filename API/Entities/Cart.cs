using API.Entities;

namespace API.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal SubTotal { get; set; }
    }
}

