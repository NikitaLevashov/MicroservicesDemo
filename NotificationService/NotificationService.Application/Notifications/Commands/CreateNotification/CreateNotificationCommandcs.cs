using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Notifications.Commands.CreateNotification
{
    public sealed record CreateNotificationCommand(
        string Type,
        string Recipient,
        string Message
    ) : IRequest<int>;

}
