using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class BasketModel
    {
        public string Username { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public BasketModel() { }

        public BasketModel(string username) => Username = username;

        public decimal TotalPrice {
            get
            {
                return Items.Aggregate(
                    (decimal)0, 
                    (price, item) => (decimal)price + item.Price * item.Quantity);
            } 
        }

    }
}
