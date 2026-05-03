
using Microsoft.AspNetCore.Mvc;
using PurchaseService.Api.Models;
using PurchaseService.Application.Interfaces;
using PurchaseService.Domain;
using PurchaseService.Infrastructure.Queues;

namespace PurchaseService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _service;

        private readonly QueuePublisher _queuePublisher;

        public PurchasesController(IPurchaseService service, QueuePublisher queuePublisher)
        {
            _service = service;
            _queuePublisher = queuePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchases = await _service.GetAllAsync();
            var result = purchases.Select(p => new PurchaseDto(
                p.Id,
                p.ClientId,
                p.TotalAmount,
                p.CreatedAt,
                p.Items.Select(i => new PurchaseItemDto(i.Id, i.ProductId, i.Quantity, i.Price)).ToList()
            ));
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchase = await _service.GetByIdAsync(id);
            if (purchase == null) return NotFound();
            return Ok(new PurchaseDto(
                purchase.Id,
                purchase.ClientId,
                purchase.TotalAmount,
                purchase.CreatedAt,
                purchase.Items.Select(i => new PurchaseItemDto(i.Id, i.ProductId, i.Quantity, i.Price)).ToList()
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseDto dto)
        {
            var purchase = new Purchase
            {
                ClientId = dto.ClientId,
                Items = dto.Items.Select(i => new PurchaseItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                TotalAmount = dto.Items.Sum(i => i.Price * i.Quantity)
            };

            var created = await _service.CreateAsync(purchase);


            await _queuePublisher.PublishAsync(new
            {
                PurchaseId = created.Id,
                ClientId = created.ClientId,
                Total = created.TotalAmount
            }, HttpContext.RequestAborted);



            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new PurchaseDto(
                created.Id,
                created.ClientId,
                created.TotalAmount,
                created.CreatedAt,
                created.Items.Select(i => new PurchaseItemDto(i.Id, i.ProductId, i.Quantity, i.Price)).ToList()
            ));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
