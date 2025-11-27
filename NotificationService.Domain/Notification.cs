
namespace NotificationService.Domain
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // Email or SMS
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Sent or Failed
    }
}
