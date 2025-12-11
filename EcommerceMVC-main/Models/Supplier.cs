using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("NhaCungCap")]
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Column("TenNhaCungCap")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("EmailLienHe")]
        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        [Column("SoDienThoai")]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Column("DiaChi")]
        [MaxLength(255)]
        public string? Address { get; set; }

        [Column("MaSoThue")]
        [MaxLength(50)]
        public string? TaxCode { get; set; }

        [Column("NgayTao")]
        public DateTime? CreatedAt { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
