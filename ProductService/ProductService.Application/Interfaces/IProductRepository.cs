using ProductService.Domain.Entities;
using static ProductService.Domain.Entities.UserTest;

namespace ProductService.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

        //Test
        Task<List<User>> TestApiAsync();
    }
}
