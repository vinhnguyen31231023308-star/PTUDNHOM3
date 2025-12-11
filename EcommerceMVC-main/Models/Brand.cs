using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("Hang")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Column("TenHang")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        [MaxLength(100)]
        public string? Slug { get; set; }

        [Column("NgayTao")]
        public DateTime? CreatedAt { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
