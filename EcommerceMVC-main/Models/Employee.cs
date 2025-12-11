using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("NhanVien")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Column("HoTen")]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        [Column("NgaySinh")]
        public DateTime? BirthDate { get; set; }

        [Column("GioiTinh")]
        [MaxLength(10)]
        public string? Gender { get; set; }

        [Column("Email")]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Column("DiaChi")]
        [MaxLength(100)]
        public string? Address { get; set; }

        [Column("DienThoaiNha")]
        [MaxLength(15)]
        public string? HomePhone { get; set; }

        [Column("DiDong")]
        [MaxLength(15)]
        public string MobilePhone { get; set; } = string.Empty;

        [Column("DuongDanAnh")]
        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [Column("NgayVaoLam")]
        public DateTime? JoinedAt { get; set; }

        public ICollection<AdminAccount> AdminAccounts { get; set; } = new List<AdminAccount>();
    }
}
