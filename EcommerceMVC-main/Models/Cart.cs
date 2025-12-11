using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models
{
    public class Cart
    {
        [Key]
        public long CartId { get; set; }

        // User sở hữu giỏ hàng
        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public DateTime? LastReminderSent { get; set; }

        // Quan hệ 1-n với CartItem
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
