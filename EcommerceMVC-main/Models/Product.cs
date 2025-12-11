using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace EcommerceMVC.Models
{
    [Table("SanPham")]
    public class Product
    {
        // ============== CỘT CHÍNH TRONG BẢNG SANPHAM ==================
        // CREATE TABLE SanPham (
        //   Id BIGINT IDENTITY(1,1) NOT NULL, ...

        [Key]
        [Column("Id")]
        public long ProductId { get; set; }   // map tới Id (BIGINT)

        [Column("DanhMucId")]
        public int? CategoryId { get; set; }  // DanhMucId INT NULL

        [Column("NhaCungCapId")]
        public int? SupplierId { get; set; }  // NhaCungCapId INT NULL

        [Column("HangId")]
        public int? BrandId { get; set; }     // HangId INT NULL

        [Required]
        [StringLength(255)]
        [Column("TenSanPham")]
        public string Name { get; set; } = null!;   // TenSanPham NVARCHAR(255) NOT NULL

        [StringLength(255)]
        [Column("Slug")]
        public string? Slug { get; set; }          // Slug VARCHAR(255) UNIQUE

        [StringLength(50)]
        [Column("MaSku")]
        public string? Sku { get; set; }           // MaSku VARCHAR(50) UNIQUE

        [StringLength(500)]
        [Column("MoTaNgan")]
        public string? ShortDescription { get; set; }  // MoTaNgan NVARCHAR(500)

        [Column("MoTaChiTiet")]
        public string? Description { get; set; }   // MoTaChiTiet NVARCHAR(MAX)

        [Column("DaBan")]
        public int SoldQuantity { get; set; }      // DaBan INT (DEFAULT 0)

        [Column("LuotXem")]
        public int ViewCount { get; set; }         // LuotXem INT (DEFAULT 0)

        [Column("NoiBat")]
        public bool? IsFeatured { get; set; }      // NoiBat BIT NULL

        [Column("HienThi")]
        public bool? IsVisible { get; set; }       // HienThi BIT NULL

        [StringLength(255)]
        [Column("TheTieuDe")]
        public string? MetaTitle { get; set; }     // TheTieuDe NVARCHAR(255)

        [StringLength(500)]
        [Column("TheMoTa")]
        public string? MetaDescription { get; set; }   // TheMoTa NVARCHAR(500)

        [Column("NgayTao")]
        public DateTime? CreatedAt { get; set; }       // NgayTao DATETIME NULL

        [Column("NgayCapNhat")]
        public DateTime? UpdatedAt { get; set; }       // NgayCapNhat DATETIME NULL

        // ================== NAVIGATION PROPERTIES =====================

        // FK -> DanhMuc
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        // FK -> NhaCungCap
        [ForeignKey(nameof(SupplierId))]
        public Supplier? Supplier { get; set; }

        // FK -> Hang
        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        // 1 sản phẩm - nhiều dung tích (DungTich)
        public ICollection<VolumeOption> VolumeOptions { get; set; } = new List<VolumeOption>();

        // 1 sản phẩm - nhiều hình ảnh (HinhAnhSanPham)
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        // Sản phẩm – Công dụng (bảng trung gian SanPhamCongDung)
        public ICollection<ProductUsage> ProductUsages { get; set; } = new List<ProductUsage>();

        // Đánh giá
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Yêu thích (SanPhamYeuThich)
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();

        // Giỏ hàng / Chi tiết đơn hàng (nếu bạn có dùng)
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // ============ THUỘC TÍNH KHÔNG MAP DB (PHỤC VỤ FORM) =========

        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // dùng khi upload 1 ảnh từ form

        // Ảnh đại diện để hiển thị
        [NotMapped]
        public string? MainImagePath =>
            ProductImages?.FirstOrDefault(i => i.IsMain == true)?.ImageUrl
            ?? ProductImages?.FirstOrDefault()?.ImageUrl;

        // ================== GIÁ “ẢO” CHO CODE CŨ ==================
        [NotMapped]
        public decimal Price
        {
            get
            {
                // Lấy giá bán của dung tích đầu tiên (hoặc thấp nhất) làm giá mặc định
                var option = VolumeOptions?
                    .OrderBy(v => v.Price)     // có thể đổi logic nếu muốn
                    .FirstOrDefault();

                return option?.Price ?? 0m;
            }
        }

        // ================== SỐ LƯỢNG ẢO CHO CODE CŨ ==================

        // Tổng số lượng tồn của *tất cả* dung tích
        [NotMapped]
        public int Quantity
            => VolumeOptions?.Sum(v => v.Stock ?? 0) ?? 0;
    }
}
