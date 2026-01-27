using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Common;
using NotificationService.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.DomainEvents
{

    public interface IDomainEventsDispatcher
    {
        Task DispatchAsync(DbContext dbContext, CancellationToken ct);
    }

    public sealed class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IPublisher _publisher;

        public DomainEventsDispatcher(IPublisher publisher) => _publisher = publisher;

        public async Task DispatchAsync(DbContext dbContext, CancellationToken ct)
        {
            // Собираем все доменные события из изменённых агрегатов
            var entities = dbContext.ChangeTracker
                .Entries()
                .Where(e => e.Entity is AggregateRoot)
                .Select(e => (AggregateRoot)e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var entitiesTest = dbContext.ChangeTracker
               .Entries()
               .ToList();

            var allEvents = entities.SelectMany(e => e.DomainEvents).ToList();

            // Очистить доменные события на агрегатах, чтобы не задублировать
            entities.ForEach(e => e.ClearDomainEvents());

            // Публикуем каждое событие через обёртку
            foreach (var domainEvent in allEvents)
            {
                var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
                var notification = Activator.CreateInstance(notificationType, domainEvent)!;
                await _publisher.Publish((INotification)notification, ct);
            }
        }
    }

}
