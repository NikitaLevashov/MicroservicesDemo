
using ClientService.Domain;
using ClientService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ClientService.Application.Interfaces;

namespace ClientService.Application.Services
{
    public class ClientServiceApp : IClientService
    {
        private readonly ClientDbContext _context;

        public ClientServiceApp(ClientDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllAsync() => await _context.Clients.ToListAsync();

        public async Task<Client?> GetByIdAsync(int id) => await _context.Clients.FindAsync(id);

        public async Task<Client> CreateAsync(Client client)
        {
            client.CreatedAt = DateTime.UtcNow;
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}
