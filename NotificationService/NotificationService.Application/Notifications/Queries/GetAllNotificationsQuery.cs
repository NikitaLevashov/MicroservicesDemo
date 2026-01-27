using MediatR;
using NotificationService.Application.Notifications.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Notifications.Queries
{
    public sealed record GetAllNotificationsQuery : IRequest<IReadOnlyList<NotificationDto>>;
}
