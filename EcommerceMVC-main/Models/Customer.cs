using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("KhachHang")]
    public class Customer
    {
        [Key]
        public long Id { get; set; }

        [Column("TenDangNhap")]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Column("MatKhau")]
        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Column("HoTen")]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Column("SoDienThoai")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Column("GioiTinh")]
        [MaxLength(10)]
        public string? Gender { get; set; }

        [Column("NgaySinh")]
        public DateTime? BirthDate { get; set; }

        [Column("AnhDaiDien")]
        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        [Column("TrangThai")]
        public bool? IsActive { get; set; }

        [Column("NgayTao")]
        public DateTime? CreatedAt { get; set; }

        [Column("NgayCapNhat")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
    }
}
