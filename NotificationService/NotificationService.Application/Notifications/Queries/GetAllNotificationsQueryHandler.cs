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

    public sealed class GetAllNotificationsQueryHandler
        : IRequestHandler<GetAllNotificationsQuery, IReadOnlyList<NotificationDto>>
    {
        private readonly INotificationReadRepository _repo;

        public GetAllNotificationsQueryHandler(INotificationReadRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<NotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(n => new NotificationDto(
                n.Id, n.Type, n.Recipient, n.Message, n.Status, n.CreatedAtUtc, n.SentAtUtc)).ToList();
        }
    }

}
