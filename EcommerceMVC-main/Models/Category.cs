using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceMVC.Models
{
    [Table("DanhMuc")]
    public class Category
    {
        [Key]
        [Column("Id")]
        public int CategoryId { get; set; }
       

        [Required, StringLength(100)]
        [Column("TenDanhMuc")]
        public string Name { get; set; } = null!;

        [Column("Slug"), StringLength(150)]
        public string? Slug { get; set; }
      

        [Column("MoTa"), StringLength(500)]
        public string? Description { get; set; }
       

        [Column("HinhAnh"), StringLength(255)]
        public string? ImageUrl { get; set; }
       

        [Column("HienThi")]
        public bool? IsActive { get; set; } = true; 

        [Column("NgayTao")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        public ICollection<Product> Products { get; set; } = new List<Product>(); // DanhMuc -> SanPham
    }
}