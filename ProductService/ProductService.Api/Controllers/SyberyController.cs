using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Interfaces;

namespace ProductService.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class SyberyController : ControllerBase
    {
        private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private readonly IProductRepository _productRepository;
        private readonly object newObj = new object();
        private readonly SemaphoreSlim slim = new(0,2);
        int count;
        public SyberyController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("sybery")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            slim.WaitAsync();
            lock (newObj) 
            {
                count++;
               dictionary.Add("1", count.ToString() );
            }
            slim.Release();
            var listProducts = await _productRepository.GetAllAsync();

            if(listProducts == null)
            {
                return BadRequest();
            }

            return listProducts;
        }
    }
}
