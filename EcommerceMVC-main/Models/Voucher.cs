using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("MaGiamGia")]
    public class Voucher
    {
        [Key]
        public int Id { get; set; }

        [Column("MaCode")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Column("MoTa")]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Column("LoaiGiamGia")]
        [MaxLength(20)]
        public string? DiscountType { get; set; } // PHAN_TRAM / TIEN_MAT

        [Column("GiaTriGiam")]
        public decimal DiscountValue { get; set; }

        [Column("GiamToiDa")]
        public decimal? MaxDiscount { get; set; }

        [Column("DonToiThieu")]
        public decimal? MinOrderValue { get; set; }

        [Column("SoLuong")]
        public int Quantity { get; set; }

        [Column("DaSuDung")]
        public int UsedQuantity { get; set; }

        [Column("GioiHanMoiNguoi")]
        public int? PerUserLimit { get; set; }

        [Column("NgayBatDau")]
        public DateTime? StartDate { get; set; }

        [Column("NgayKetThuc")]
        public DateTime? EndDate { get; set; }

        [Column("TrangThai")]
        public bool? IsActive { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        // Bổ sung: Thuộc tính kiểm tra tính khả dụng (NotMapped)
        [NotMapped]
        public bool IsAvailable
        {
            get
            {
                // 1. Phải đang hoạt động (IsActive)
                // 2. Chưa hết số lượng (Quantity > UsedQuantity)
                // 3. Đang trong thời gian áp dụng (StartDate <= Now <= EndDate)

                bool isQuantityRemaining = Quantity > UsedQuantity;
                bool isTimeValid = StartDate.HasValue && StartDate.Value <= DateTime.Now &&
                                   (!EndDate.HasValue || EndDate.Value >= DateTime.Now);

                return (IsActive ?? false) && isQuantityRemaining && isTimeValid;
            }
        }
    }
}
