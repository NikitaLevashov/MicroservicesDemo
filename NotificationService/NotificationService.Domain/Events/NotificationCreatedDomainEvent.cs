using NotificationService.Domain.Abstractions;
using NotificationService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Events
{
    public sealed record NotificationCreatedDomainEvent(
        Notification Notification
    ) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

}
