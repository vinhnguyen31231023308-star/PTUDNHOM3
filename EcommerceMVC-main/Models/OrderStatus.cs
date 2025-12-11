using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("TrangThaiDonHang")]
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Column("TenTrangThai")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
