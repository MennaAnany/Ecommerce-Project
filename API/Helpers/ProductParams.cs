namespace API.Helpers
{
    public class ProductParams : PaginationParams
    {
        public string Category { get; set; }
        public string SearchFilter { get; set; }
        public string PriceRange { get; set; }
    }
}
