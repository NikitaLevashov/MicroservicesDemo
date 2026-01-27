using NotificationService.Application.Abstraction;
using NotificationService.Infrastructure.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly NotificationDbContext _db;
        private readonly IDomainEventsDispatcher _dispatcher;

        public UnitOfWork(NotificationDbContext db, IDomainEventsDispatcher dispatcher)
        {
            _db = db;
            _dispatcher = dispatcher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            var result = await _db.SaveChangesAsync(ct);
            await _dispatcher.DispatchAsync(_db, ct); // публикуем доменные события после успешной транзакции
            return result;
        }

    }
}
