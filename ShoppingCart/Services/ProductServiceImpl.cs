using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class ProductServiceImpl : ProductService
    {
        private DatabaseContext db;

        public ProductServiceImpl( DatabaseContext _db)
        {
            db = _db;
        }
        public List<ProductCart> findAll()
        {
            return db.ProductCarts.ToList();
        }

        public ProductCart findById(int id)
        {
            return db.ProductCarts.Find(id);
        }
    }
}
