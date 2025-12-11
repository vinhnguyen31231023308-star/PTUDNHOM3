using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("HinhAnhSanPham")]
    public class ProductImage
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("SanPhamId")]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required, StringLength(255)]
        [Column("DuongDanAnh")]
        public string ImageUrl { get; set; } = null!;

        [Column("LaAnhChinh")]
        public bool? IsMain { get; set; }

        [Column("ThuTu")]
        public int? DisplayOrder { get; set; }
    }
}
