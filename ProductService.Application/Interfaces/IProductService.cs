using ProductService.Infrastructure.Persistence;

namespace ProductService.Application.Interfaces
{
    public interface IProductService
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
