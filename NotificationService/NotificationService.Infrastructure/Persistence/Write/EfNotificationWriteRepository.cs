using NotificationService.Application.Abstractions;
using NotificationService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence.Write
{
    public sealed class EfNotificationWriteRepository : INotificationWriteRepository
    {
        private readonly NotificationDbContext _db;
        public EfNotificationWriteRepository(NotificationDbContext db) => _db = db;

        public Task AddAsync(Notification notification, CancellationToken ct)
        {
            _db.Notifications.Add(notification);
            return Task.CompletedTask;
        }

        public Task<Notification?> FindAsync(int id, CancellationToken ct)
            => _db.Notifications.FindAsync(new object?[] { id }, ct).AsTask();
    }

}
