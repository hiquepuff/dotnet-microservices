using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<BasketModel> GetBasket(string username);
        Task<BasketModel> UpdateBasket(BasketModel basket);
        Task DeleteBasket(string username);
    }
}
