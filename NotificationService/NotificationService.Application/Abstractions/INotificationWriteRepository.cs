using NotificationService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Abstractions
{
    public interface INotificationWriteRepository
    {
        Task AddAsync(Notification notification, CancellationToken ct);
        Task<Notification?> FindAsync(int id, CancellationToken ct);
    }

}
