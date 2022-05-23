using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache) => _redisCache = redisCache;

        public async Task DeleteBasket(string username) => await _redisCache.RemoveAsync(username);

        public async Task<BasketModel> GetBasket(string username)
        {
            var basket = await _redisCache.GetStringAsync(username);

            if (String.IsNullOrEmpty(basket)) return null;

            return JsonConvert.DeserializeObject<BasketModel>(basket);
        }

        public async Task<BasketModel> UpdateBasket(BasketModel basket)
        {
            await _redisCache.SetStringAsync(basket.Username, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.Username);
        }
    }
}
