namespace ProductService.Api.Controllers
{
    using global::ProductService.Api.Models;
    using global::ProductService.Api.TestFolder;
    using global::ProductService.Application.Commands;
    using global::ProductService.Application.Interfaces;
    using global::ProductService.Domain.Entities;
    using global::ProductService.Infrastructure.Persistence;
    using MassTransit;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Serilog;
    using SharedMessaging.Contracts;
    using System.Security.Claims;

    //using PurchaseService.Api.Models;
    //using PurchaseService.Application.Interfaces;

    namespace ProductService.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        //[Authorize]
        public class ProductsController : ControllerBase
        {
            private readonly IMediator _mediator;

            private readonly IProductRepository _service;
            //private readonly ClaimsPrincipal _claimsPrincipal;
            private readonly IMemoryCache _memoryCache;

            //private readonly IPublishEndpoint _publish;

            public ProductsController(
                IMediator mediator,
                IProductRepository service,
                ClaimsPrincipal claimsPrincipal,
                IMemoryCache memoryCache
                /*IPublishEndpoint publish*/)
            {
                //_publish = publish;
                _service = service;
                //_claimsPrincipal = claimsPrincipal;
                _memoryCache = memoryCache;
                _mediator = mediator;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                Log.Information("GetAll products called");

                var isProductsCached = _memoryCache.TryGetValue("allProducts", out IEnumerable<Product>? cacheProducts);

                if(isProductsCached)
                {
                    var result1 = cacheProducts?.Select(p => new ProductDto(p.Id, p.Name, p.Price.Value, p.StockQuantity));
                    return Ok(result1);
                }

                var products = await _service.GetAllAsync();

                if (products != null)
                {
                    _memoryCache.Set("allProducts", products, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                // test claims
                //var t = _claimsPrincipal;

                var result = products.Select(p => new ProductDto(p.Id, p.Name, p.Price.Value, p.StockQuantity));
                return Ok(result);
            }

            [HttpGet("test")]
            //[FakeNotFoundResourceFilter]
            [SimpleResourceFilter]
            public async Task<IActionResult> TestApi()
            {
                var result = await _service.TestApiAsync();

                return Ok(result);
            }

            [HttpGet("{id:int}")]
            public async Task<IActionResult> GetById(int id)
            {
                var product = await _service.GetByIdAsync(id);

                throw new FileNotFoundException();
                if (product == null) return NotFound();
                return Ok(new ProductDto(product.Id, product.Name, product.Price.Value, product.StockQuantity));
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateProductDto dto, CancellationToken ct)
            {

                var createdProduct = await _mediator.Send(new CreateUpdatedCommand(
                            dto.Name,
                            dto.Price,
                            dto.StockQuantity
                        ), ct);

                //var product = new Product
                //{
                //    Name = dto.Name,
                //    Price = dto.Price,
                //    StockQuantity = dto.StockQuantity
                //};

                //var created = await _service.CreateAsync(product);


                //// NEW: формируем событие и публикуем его в RabbitMQ
                //var evt = new ProductCreated(
                //    EventId: Guid.NewGuid(),
                //    OccurredAtUtc: DateTime.UtcNow,
                //    Version: "1.0",
                //    ProductId: created.Id,
                //    Name: created.Name,
                //    Price: created.Price.Value,
                //    StockQuantity: created.StockQuantity
                //);

                //await _publish.Publish(evt, ct); // отправка события

                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, new ProductDto(createdProduct.Id, createdProduct.Name, createdProduct.Price.Value, createdProduct.StockQuantity));
            }

            [HttpPut("{id:int}")]
            public async Task<IActionResult> Update(int id, UpdateProductDto dto, CancellationToken ct)
            {
                var updatedProduct = await _mediator.Send(new UpdateProductCommand(
                        id,
                        dto.Name,
                        dto.Price,
                        dto.StockQuantity
                    ), ct);

                if (updatedProduct is null)
                {
                    return BadRequest();
                }


                //var product = await _service.GetByIdAsync(id);
                //if (product == null) return NotFound();

                //product.Name = dto.Name;
                //product.Price = dto.Price;
                //product.StockQuantity = dto.StockQuantity;

                //await _service.UpdateAsync(product);
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

}
