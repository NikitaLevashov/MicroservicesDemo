using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Abstractions;
using NotificationService.Applications.Abstractions;
using NotificationService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence.Read
{
    public sealed class EfNotificationReadRepository : INotificationReadRepository
    {
        private readonly NotificationDbContext _db;
        public EfNotificationReadRepository(NotificationDbContext db) => _db = db;

        public async Task<IReadOnlyList<Notification>> GetAllAsync(CancellationToken ct)
            => await _db.Notifications.AsNoTracking()
               .OrderByDescending(x => x.CreatedAtUtc)
               .ToListAsync(ct);

        public async Task<Notification?> GetByIdAsync(int id, CancellationToken ct)
            => await _db.Notifications.AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

}
