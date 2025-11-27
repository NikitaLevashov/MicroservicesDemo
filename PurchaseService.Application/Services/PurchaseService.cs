
using PurchaseService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PurchaseService.Application.Interfaces;

namespace PurchaseService.Application.Services
{
    public class PurchaseServiceApp : IPurchaseService
    {
        private readonly PurchaseDbContext _context;

        public PurchaseServiceApp(PurchaseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllAsync() => await _context.Purchases.Include(p => p.Items).ToListAsync();

        public async Task<Purchase?> GetByIdAsync(int id) => await _context.Purchases.Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Purchase> CreateAsync(Purchase purchase)
        {
            purchase.CreatedAt = DateTime.UtcNow;
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }

        public async Task UpdateAsync(Purchase purchase)
        {
            _context.Purchases.Update(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }
        }
    }
}
