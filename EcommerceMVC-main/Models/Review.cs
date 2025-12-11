using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("DanhGia")]
    public class Review
    {
        [Key]
        [Column("Id")]
        public long ReviewId { get; set; }

        [Column("SanPhamId")]
        public long ProductId { get; set; }

        [Column("KhachHangId")]
        public long CustomerId { get; set; }

        [Column("DonHangId")]
        public long? OrderId { get; set; }

        [Column("SoSao")]
        public int Rating { get; set; }

        [Column("TieuDe")]
        [StringLength(100)]
        public string? Title { get; set; } // Có giới hạn 100 ký tự

        [Column("HinhAnh")]
        [StringLength(int.MaxValue)] // nvarchar(max)
        public string? Image { get; set; }

        [Column("NoiDung")]
        [StringLength(500)]
        public string? Content { get; set; }

        [Column("DaDuyet")]
        public bool? IsApproved { get; set; }

        [Column("NgayTao")]
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Product Product { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
