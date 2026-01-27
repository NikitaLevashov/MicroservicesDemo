using NotificationService.Domain.Abstractions;
using NotificationService.Domain.Events;

namespace NotificationService.Domain.Notifications
{
    public sealed class Notification : AggregateRoot
    {
        public int Id { get; private set; }
        public string Type { get; private set; } = default!;
        public string Recipient { get; private set; } = default!;
        public string Message { get; private set; } = default!;
        public string Status { get; private set; } = "Pending";
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? SentAtUtc { get; private set; }

        private Notification() { }

        public Notification(string type, string recipient, string message)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type is required");
            if (string.IsNullOrWhiteSpace(recipient)) throw new ArgumentException("Recipient is required");
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message is required");

            Type = type;
            Recipient = recipient;
            Message = message;
            Status = "Pending";
            CreatedAtUtc = DateTime.UtcNow;

            // Подняли доменное событие (Id пока неизвестен, но мы публикуем события уже ПОСЛЕ SaveChanges)
            Raise(new NotificationCreatedDomainEvent(this));
        }

        public void MarkSent()
        {
            var old = Status;
            Status = "Sent";
            SentAtUtc = DateTime.UtcNow;
            Raise(new NotificationStatusChangedDomainEvent(this, old, Status));
        }

        public void MarkFailed()
        {
            var old = Status;
            Status = "Failed";
            Raise(new NotificationStatusChangedDomainEvent(this, old, Status));
        }
    }


}
