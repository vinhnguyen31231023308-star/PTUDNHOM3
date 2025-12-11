using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models
{
    public class Order
    {
        public int OrderId { get; set; }          // KHÓA CHÍNH, int

        [Required]
        public string UserId { get; set; } = null!;

        public DateTime OrderDate { get; set; }   // ngày đặt hàng

        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }  // TỔNG TIỀN ĐƠN HÀNG

        [Required, StringLength(200)]
        public string FullName { get; set; } = null!;

        [Required, StringLength(500)]
        public string Address { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, Phone]
        public string Phone { get; set; } = null!;

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; } = null!;

        [Required, StringLength(50)]
        public string Status { get; set; } = null!;

        [StringLength(100)]
        public string? TrackingNumber { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
