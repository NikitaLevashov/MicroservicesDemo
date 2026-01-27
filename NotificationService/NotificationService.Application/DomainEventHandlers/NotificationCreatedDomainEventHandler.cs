using MassTransit;
using MediatR;
using NotificationService.Application.Common;
using NotificationService.Domain.Events;
using SharedMessaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.DomainEventHandlers
{

    public sealed class NotificationCreatedDomainEventHandler
        : INotificationHandler<DomainEventNotification<NotificationCreatedDomainEvent>>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
            => _publishEndpoint = publishEndpoint;

        public async Task Handle(DomainEventNotification<NotificationCreatedDomainEvent> notification, CancellationToken ct)
        {
            var e = notification.DomainEvent;
            var n = e.Notification;

            // Интеграционное событие наружу (другие сервисы могут подписаться)
            var integrationEvent = new NotificationCreated
            {
                NotificationId = n.Id,
                Type = n.Type,
                Recipient = n.Recipient,
                Message = n.Message,
                Status = n.Status,
                CreatedAtUtc = n.CreatedAtUtc
            };

            await _publishEndpoint.Publish(integrationEvent, ct);
        }
    }

}
