using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface CartService
    {

        public int Exist(int id, List<Item> cart);

        public double Total(List<Item> cart);
    }
}
