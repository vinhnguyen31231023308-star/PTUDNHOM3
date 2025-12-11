using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("CongDung")]
    public class UsageTag
    {
        [Key]
        public int Id { get; set; }

        [Column("TenCongDung")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("Slug")]
        [MaxLength(150)]
        public string? Slug { get; set; }

        [Column("LoaiTag")]
        [MaxLength(50)]
        public string? TagType { get; set; }

        [Column("MoTa")]
        [MaxLength(500)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<ProductUsage> ProductUsages { get; set; } = new List<ProductUsage>();
    }
}
