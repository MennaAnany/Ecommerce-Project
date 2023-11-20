using API.Entities;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryNameAsync(string name);
    }
}
