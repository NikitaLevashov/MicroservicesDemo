using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Notifications.DTO
{
    public sealed record NotificationDto(
        int Id,
        string Type,
        string Recipient,
        string Message,
        string Status,
        DateTime CreatedAtUtc,
        DateTime? SentAtUtc
    );

}
