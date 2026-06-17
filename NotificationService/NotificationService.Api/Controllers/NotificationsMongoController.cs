using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models.Requests;
using NotificationService.Infrastructure.Persistence.Collections;
using NotificationService.Infrastructure.Persistence.Repositories;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("api/notifications-mongo")]
    public class NotificationsMongoController : ControllerBase
    {
        private readonly NotificationMongoRepository _repo;

        public NotificationsMongoController(NotificationMongoRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationRequest request)
        {
            var doc = new NotificationDocument
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(doc);

            return Ok(doc.Id);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var items = await _repo.GetByUser(userId);
            return Ok(items);
        }


        [HttpGet("debug")]
        public async Task<IActionResult> Debug()
        {
            var doc = new NotificationDocument
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "debug",
                Message = "DEBUG INSERT",
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(doc);

            return Ok("Inserted");
        }

    }
}
