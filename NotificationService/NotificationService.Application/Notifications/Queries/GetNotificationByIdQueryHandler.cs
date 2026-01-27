using MediatR;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Notifications.DTO;
using NotificationService.Applications.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Notifications.Queries
{
    public sealed class GetNotificationByIdQueryHandler
        : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
    {
        private readonly INotificationReadRepository _repo;

        public GetNotificationByIdQueryHandler(INotificationReadRepository repo) => _repo = repo;

        public async Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken ct)
        {
            var n = await _repo.GetByIdAsync(request.Id, ct);
            return n is null ? null
                : new NotificationDto(n.Id, n.Type, n.Recipient, n.Message, n.Status, n.CreatedAtUtc, n.SentAtUtc);
        }
    }

}
