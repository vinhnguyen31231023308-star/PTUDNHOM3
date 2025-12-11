using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("SanPhamYeuThich")]
    public class FavoriteProduct
    {
        [Key]
        public long Id { get; set; }

        [Column("KhachHangId")]
        public long CustomerId { get; set; }

        [Column("SanPhamId")]
        public long ProductId { get; set; }

        [Column("NgayThem")]
        public DateTime? AddedAt { get; set; }

        public Customer Customer { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
