using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("LienHeTuVan")]
    public class ConsultationRequest
    {
        [Key]
        public int Id { get; set; }

        [Column("HoTen")]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Column("SoDienThoai")]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("NoiDung")]
        public string? Content { get; set; }

        [Column("TrangThai")]
        [MaxLength(50)]
        public string? Status { get; set; }

        [Column("NgayGui")]
        public DateTime? SentAt { get; set; }
    }
}
