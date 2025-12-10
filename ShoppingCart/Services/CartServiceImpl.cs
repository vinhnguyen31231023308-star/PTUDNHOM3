using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class CartServiceImpl : CartService
    {
        public int Exist(int id, List<Item> cart)
        {
            return cart.FindIndex(i => i.Id == id);
        }

        public double Total(List<Item> cart)
        {
            return cart.Sum(i => i.Price * i.Quantity);
        }
    }
}
