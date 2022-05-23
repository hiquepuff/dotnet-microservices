using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketCache;

        public BasketController(IBasketRepository basketCache) => _basketCache = basketCache;

        [HttpGet("{username}")]
        [ProducesResponseType( typeof(BasketModel), (int)HttpStatusCode.OK )]
        public async Task<ActionResult<BasketModel>> GetBasket(string username)
        {
            var basket = await _basketCache.GetBasket(username);

            return Ok(basket ?? new BasketModel(username));
        }

        [HttpPost]
        [ProducesResponseType( typeof(BasketModel), (int)HttpStatusCode.OK )]
        public async Task<ActionResult<BasketModel>> UpdateBasket([FromBody] BasketModel basket) =>
            Ok(await _basketCache.UpdateBasket(basket));

        [HttpDelete]
        [ProducesResponseType( (int)HttpStatusCode.OK )]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _basketCache.DeleteBasket(username);

            return Ok();
        }
    }
}
