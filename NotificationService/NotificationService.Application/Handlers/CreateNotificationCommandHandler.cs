using MediatR;
using NotificationService.Application.Abstraction;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Notifications.Commands.CreateNotification;
using NotificationService.Domain.Notifications;

namespace NotificationService.Application.Handlers
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
