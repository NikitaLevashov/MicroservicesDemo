using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedMessaging.Contracts
{
    public record NotificationCreated
    {
        public int NotificationId { get; set; }
        public string Type { get; set; } = default!;
        public string Recipient { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
    }

}
