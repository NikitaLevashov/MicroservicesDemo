
using NotificationService.Domain;
using NotificationService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Services
{
    public class NotificationServiceApp : INotificationService
    {
        private readonly NotificationDbContext _context;

        public NotificationServiceApp(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllAsync() => await _context.Notifications.ToListAsync();

        public async Task<Notification?> GetByIdAsync(int id) => await _context.Notifications.FindAsync(id);

        public async Task<Notification> CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
