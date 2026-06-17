using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using NotificationService.Infrastructure.Persistence.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence.MongoDB
{

    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config["Mongo:ConnectionString"]);
            _database = client.GetDatabase(config["Mongo:Database"]);
        }

        public IMongoCollection<NotificationDocument> Notifications =>
            _database.GetCollection<NotificationDocument>("notifications");

        public IMongoCollection<NewNotificationDocument> NewNotifications =>
           _database.GetCollection<NewNotificationDocument>("notifications");
    }


}
