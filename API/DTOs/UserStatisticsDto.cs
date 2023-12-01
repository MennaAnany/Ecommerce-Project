namespace API.DTOs
{
    public class UserStatisticsDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int TotalOrders { get; set; }
        public StatisticsDto MostBoughtProduct { get; set; }
    }
}
