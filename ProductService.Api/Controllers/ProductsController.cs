namespace ProductService.Api.Controllers
{
    using global::ProductService.Api.Models;
    using global::ProductService.Api.TestFolder;
    using global::ProductService.Application.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    //using PurchaseService.Api.Models;
    //using PurchaseService.Application.Interfaces;

    namespace ProductService.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class ProductsController : ControllerBase
        {
            private readonly IProductService _service;

            public ProductsController(IProductService service)
            {
                _service = service;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var products = await _service.GetAllAsync();
                var result = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.StockQuantity));
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
                if (product == null) return NotFound();
                return Ok(new ProductDto(product.Id, product.Name, product.Price, product.StockQuantity));
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateProductDto dto)
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity
                };

                var created = await _service.CreateAsync(product);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ProductDto(created.Id, created.Name, created.Price, created.StockQuantity));
            }

            [HttpPut("{id:int}")]
            public async Task<IActionResult> Update(int id, UpdateProductDto dto)
            {
                var product = await _service.GetByIdAsync(id);
                if (product == null) return NotFound();

                product.Name = dto.Name;
                product.Price = dto.Price;
                product.StockQuantity = dto.StockQuantity;

                await _service.UpdateAsync(product);
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
