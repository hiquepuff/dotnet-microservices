using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
          
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() =>
            Ok(await _repository.GetProducts());

        [HttpGet("id:length(24)")]
        [ProducesResponseType( (int)HttpStatusCode.NotFound )]
        [ProducesResponseType( typeof(Product), (int)HttpStatusCode.OK )]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product with id {id} was not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType( (int)HttpStatusCode.NotFound )]
        [ProducesResponseType( typeof(Product), (int)HttpStatusCode.OK )]
        public async Task<ActionResult<Product>> GetProductsByCategory(string category)
        {
            var products = await _repository.GetProductsByCategory(category);

            if (products.ToList().Count < 1) return NotFound($"There is no products categorized as {category}");

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType( typeof(Product), (int)HttpStatusCode.OK )]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product) => 
            Ok(await _repository.UpdateProduct(product));

        [HttpDelete("(id:length(24)")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id) => 
            Ok(await _repository.DeleteProduct(id));
    }
}
