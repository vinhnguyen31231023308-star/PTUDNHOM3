using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("DungTich")]
    public class VolumeOption
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("SanPhamId")]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Column("TenDungTich")]
        [StringLength(50)]
        public string? Name { get; set; }

        [Column("GiaBan", TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        [Column("GiaGiam", TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        [Column("SoLuongTon")]
        public int? Stock { get; set; }

        [Column("DaBan")]
        public int? Sold { get; set; }

        // ================== THUỘC TÍNH TÍNH TOÁN (Tùy chọn) ==================

        // Giá đang được bán (Ưu tiên SalePrice nếu có)
        [NotMapped]
        public decimal? CurrentPrice => SalePrice.HasValue ? SalePrice : Price;
    }
}
