
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models;
using NotificationService.Application.Interfaces;
using NotificationService.Domain;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _service.GetAllAsync();
            var result = notifications.Select(n => new NotificationDto(n.Id, n.Type, n.Recipient, n.Message, n.Status));
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _service.GetByIdAsync(id);
            if (notification == null) return NotFound();
            return Ok(new NotificationDto(notification.Id, notification.Type, notification.Recipient, notification.Message, notification.Status));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                Type = dto.Type,
                Recipient = dto.Recipient,
                Message = dto.Message,
                Status = "Pending"
            };

            var created = await _service.CreateAsync(notification);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new NotificationDto(created.Id, created.Type, created.Recipient, created.Message, created.Status));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateNotificationDto dto)
        {
            var notification = await _service.GetByIdAsync(id);
            if (notification == null) return NotFound();

            notification.Type = dto.Type;
            notification.Recipient = dto.Recipient;
            notification.Message = dto.Message;
            notification.Status = dto.Status;

            await _service.UpdateAsync(notification);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
