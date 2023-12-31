﻿
using API.Entities.API.Entities;
namespace API.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; }
    }
}
