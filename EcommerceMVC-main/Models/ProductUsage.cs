using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("SanPhamCongDung")]
    public class ProductUsage
    {
        [Column("SanPhamId")]
        public long ProductId { get; set; }

        [Column("CongDungId")]
        public int UsageTagId { get; set; }

        public Product Product { get; set; } = default!;
        public UsageTag UsageTag { get; set; } = default!;
    }
}
