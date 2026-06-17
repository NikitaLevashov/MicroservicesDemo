using MongoDB.Driver;
using NotificationService.Infrastructure.Persistence.Collections;
using NotificationService.Infrastructure.Persistence.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence.Repositories
{
    public class NotificationMongoRepository
    {
        private readonly IMongoCollection<NotificationDocument> _collection;

        public NotificationMongoRepository(MongoContext context)
        {
            _collection = context.Notifications;
        }

        public async Task CreateAsync(NotificationDocument doc)
        {
            var task = _collection.InsertOneAsync(doc);

            await task;
        }

        public async Task<List<NotificationDocument>> GetByUser(string userId)
        {
            return await _collection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        //public async Task CreateNewAsync(NewNotificationDocument doc)
        //{
        //    var task = _collection.InsertOneAsync(doc);

        //    await task;
        //}
    }
}
