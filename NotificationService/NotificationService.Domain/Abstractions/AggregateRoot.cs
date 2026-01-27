using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Abstractions
{
    public class AggregateRoot
    {

        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void Raise(IDomainEvent @event) => _domainEvents.Add(@event);

        public void ClearDomainEvents() => _domainEvents.Clear();

    }
}
