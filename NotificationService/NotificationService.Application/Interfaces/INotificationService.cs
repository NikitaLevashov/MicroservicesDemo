using NotificationService.Domain.Notifications;

namespace NotificationService.Application.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification> CreateAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
    }
}
