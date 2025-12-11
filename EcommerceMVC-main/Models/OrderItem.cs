using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models
{
    public class OrderItem
    {
        [Key]
        public long OrderItemId { get; set; }

        public long OrderId { get; set; }
        public Order? Order { get; set; }

        public long ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
