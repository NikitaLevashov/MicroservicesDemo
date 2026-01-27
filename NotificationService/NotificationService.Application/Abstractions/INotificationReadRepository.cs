using NotificationService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Applications.Abstractions
{
    public interface INotificationReadRepository
    {
        Task<IReadOnlyList<Notification>> GetAllAsync(CancellationToken ct);
        Task<Notification?> GetByIdAsync(int id, CancellationToken ct);
    }

}
