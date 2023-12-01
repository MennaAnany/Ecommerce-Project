using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<ProductDto> GetProductBySlugAsync(string slug);
        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);
        Task<StatisticsDto> FindMostBoughtProduct(List<Order> orders);
    }
}
