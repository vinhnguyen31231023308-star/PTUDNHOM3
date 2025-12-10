using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Services;
using Newtonsoft.Json;

namespace ShoppingCart.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private ProductService productService;
        private CartService cartService;

        public CartController(ProductService _productService, CartService _cartService)
        {
            productService = _productService;
            cartService = _cartService;
        }
        [Route("index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Item> cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                ViewBag.cart = cart;
                ViewBag.total = cartService.Total(cart);
            }
            return View();
        }
        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var productcart = productService.findById(id);
            if(HttpContext.Session.GetString("cart") == null)
            {
                var item = new Item(productcart);
                item.Quantity = 1;
                var cart = new List<Item>();
                cart.Add(item);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

            }
            else
            {
                List<Item> cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                ViewBag.cart = cart;
                var index = cartService.Exist(id, cart);
                if (index == -1)
                {
                    var item = new Item(productcart);
                    item.Quantity = 1;
                    cart.Add(item);
                }
                else
                {
                    cart[index].Quantity++;
                }
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("index");
        }
        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            var index = cartService.Exist(id, cart);
            cart.RemoveAt(index);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("index");
        }
    }
}
