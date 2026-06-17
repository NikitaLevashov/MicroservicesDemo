using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence.Collections
{
    public class NewNotificationDocument
    {

        // ✅ основной ID (идёт в _id в Mongo)
        //[BsonId]
        //[BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        // ✅ тип уведомления
        public string Type { get; set; } = default!;

        // ✅ пользователь (может быть null для system)
        public string? UserId { get; set; }

        // ✅ статус (удобно для real cases)
        public string Status { get; set; } = "NEW";  // NEW / SENT / FAILED

        // ✅ гибкие данные
        public object? Payload { get; set; }

        // ✅ мета
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }

    }
}
