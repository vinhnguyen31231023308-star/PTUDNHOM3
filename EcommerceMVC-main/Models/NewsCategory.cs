using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("DanhMucTinTuc")]
    public class NewsCategory
    {
        [Key]
        public int Id { get; set; }

        [Column("TenDanhMuc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("TrangThai")]
        public bool? IsActive { get; set; }

        public ICollection<NewsArticle> Articles { get; set; } = new List<NewsArticle>();
    }
}
