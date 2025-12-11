using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models
{
    public class CartItem
    {
        [Key]
        public long CartItemId { get; set; }

        public long CartId { get; set; }
        public Cart? Cart { get; set; }

        public long ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        // Giá t?i th?i ?i?m thêm vào gi?
        public decimal Price { get; set; }
    }
}
