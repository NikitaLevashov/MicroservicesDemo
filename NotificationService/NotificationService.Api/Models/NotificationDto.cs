
namespace NotificationService.Api.Models
{
    public record NotificationDto(int Id, string Type, string Recipient, string Message, string Status);
    public record CreateNotificationDto(string Type, string Recipient, string Message);
    public record UpdateNotificationDto(string Type, string Recipient, string Message, string Status);
}
