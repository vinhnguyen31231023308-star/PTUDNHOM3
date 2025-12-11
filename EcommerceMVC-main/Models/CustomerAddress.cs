using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("DiaChiKhachHang")]
    public class CustomerAddress
    {
        [Key]
        public long Id { get; set; }

        [Column("KhachHangId")]
        public long CustomerId { get; set; }

        [Column("TenNguoiNhan")]
        [MaxLength(100)]
        public string? ReceiverName { get; set; }

        [Column("SoDienThoaiNguoiNhan")]
        [MaxLength(15)]
        public string? ReceiverPhone { get; set; }

        [Column("TinhThanh")]
        [MaxLength(100)]
        public string? Province { get; set; }

        [Column("Phuong")]
        [MaxLength(100)]
        public string? Ward { get; set; }

        [Column("DiaChiCuThe")]
        [MaxLength(255)]
        public string? DetailAddress { get; set; }

        [Column("LoaiDiaChi")]
        [MaxLength(20)]
        public string? AddressType { get; set; }

        [Column("MacDinh")]
        public bool? IsDefault { get; set; }

        public Customer Customer { get; set; } = default!;
    }
}
