using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("TinTuc")]
    public class NewsArticle
    {
        [Key]
        public long Id { get; set; }

        [Column("DanhMucTinTucId")]
        public int? NewsCategoryId { get; set; }

        [Column("TieuDe")]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Column("Slug")]
        [MaxLength(255)]
        public string? Slug { get; set; }

        [Column("HinhAnh")]
        [MaxLength(255)]
        public string? ThumbnailUrl { get; set; }

        [Column("TomTat")]
        [MaxLength(500)]
        public string? Summary { get; set; }

        [Column("NoiDung")]
        public string? Content { get; set; }

        [Column("LuotXem")]
        public int ViewCount { get; set; }

        [Column("TacGiaId")]
        public long? AuthorCustomerId { get; set; }

        [Column("NgayXuatBan")]
        public DateTime? PublishedAt { get; set; }

        public NewsCategory? NewsCategory { get; set; }
        public Customer? AuthorCustomer { get; set; }
    }
}
