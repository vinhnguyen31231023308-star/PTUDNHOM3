using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface ProductService
    {
        public List<ProductCart> findAll();

        public ProductCart findById(int id);
    }
}
