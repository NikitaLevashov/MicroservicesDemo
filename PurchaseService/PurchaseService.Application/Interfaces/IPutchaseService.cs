
namespace PurchaseService.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task<List<Purchase>> GetAllAsync();
        Task<Purchase?> GetByIdAsync(int id);
        Task<Purchase> CreateAsync(Purchase purchase);
        Task UpdateAsync(Purchase purchase);
        Task DeleteAsync(int id);
    }
}
