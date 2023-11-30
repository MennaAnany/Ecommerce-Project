using API.DTOs;

namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal SubTotal { get; set; }
    }
}
