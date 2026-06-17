namespace NotificationService.Api.Models.Requests
{
    public class CreateNotificationRequest
    {
        public string UserId { get; set; }
        public string Message { get; set; }
    }

}
