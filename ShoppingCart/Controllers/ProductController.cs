using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    [Route("productcart")]
    public class ProductController : Controller
    {
        private ProductService productService;

        public ProductController(ProductService _productService)
        {
            productService = _productService;
        }
        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index()
        {
            ViewBag.productcarts = productService.findAll();
            return View();
        }
    }
}
