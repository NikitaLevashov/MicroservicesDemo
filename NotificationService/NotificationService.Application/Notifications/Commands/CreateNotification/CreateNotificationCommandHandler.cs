using MediatR;
using NotificationService.Application.Abstraction;
using NotificationService.Application.Abstractions;
using NotificationService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Notifications.Commands.CreateNotification
{

    public sealed class CreateNotificationCommandHandler
        : IRequestHandler<CreateNotificationCommand, int>
    {
        private readonly INotificationWriteRepository _repo;
        private readonly IUnitOfWork _uow;

        public CreateNotificationCommandHandler(INotificationWriteRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<int> Handle(CreateNotificationCommand request, CancellationToken ct)
        {
            var notification = new Notification(request.Type, request.Recipient, request.Message);
            await _repo.AddAsync(notification, ct);
            await _uow.SaveChangesAsync(ct);
            return notification.Id;
        }
    }

}
