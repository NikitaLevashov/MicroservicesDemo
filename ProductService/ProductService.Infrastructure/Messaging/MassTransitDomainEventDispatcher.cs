using MassTransit;
using ProductService.Application.Abstractions;
using ProductService.Domain.Abstractions;
using ProductService.Domain.Events;
using SharedMessaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Messaging
{

    public class MassTransitDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IPublishEndpoint _publish;

        public MassTransitDomainEventDispatcher(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct)
        {
            foreach (var domainEvent in events)
            {
                switch (domainEvent)
                {
                    case ProductCreatedDomainEvent e:
                        var messageCreated = new ProductCreated(
                            EventId: Guid.NewGuid(),
                            OccurredAtUtc: DateTime.UtcNow,
                            Version: "1.0",
                            ProductId: e.Id,
                            Name: e.Name,
                            Price: e.Price,
                            StockQuantity: e.StockQuantity
                        );
                        await _publish.Publish(messageCreated, ct);
                        break;
                    case ProductUpdatedDomainEvent e:
                        var messageUpdated = new ProductUpdated(
                            EventId: Guid.NewGuid(),
                            OccurredAtUtc: DateTime.UtcNow,
                            Version: "1.0",
                            ProductId: e.ProductId,
                            Name: e.Name,
                            Price: e.Price,
                            StockQuantity: e.StockQuantity
                        );
                        await _publish.Publish(messageUpdated, ct);
                        break;
                }
            }
        }
    }

}
