using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("TaiKhoanQuanTri")]
    public class AdminAccount
    {
        [Key]
        public int Id { get; set; }

        [Column("NhanVienId")]
        public int EmployeeId { get; set; }

        [Column("TenDangNhap")]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Column("MatKhau")]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [Column("LoaiQuyen")]
        public int? RoleType { get; set; } // 1: Admin, 2: Sales, 3: Warehouse

        [Column("GhiChu")]
        [MaxLength(255)]
        public string? Note { get; set; }

        [Column("TrangThai")]
        public bool? IsActive { get; set; }

        public Employee Employee { get; set; } = default!;
    }
}
