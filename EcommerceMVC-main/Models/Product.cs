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

        // ================== THUỘC TÍNH TÍNH TOÁN ==================

        // 1. ĐIỂM ĐÁNH GIÁ TRUNG BÌNH (Giải quyết lỗi CS1061)
        [NotMapped]
        public double AverageRating
        {
            get
            {
                // Đảm bảo Reviews đã được load và có ít nhất 1 Rating có giá trị
                if (Reviews == null || !Reviews.Any() || !Reviews.Any(r => r.Rating.HasValue))
                {
                    return 0;
                }

                // Tính trung bình trên các giá trị Rating hợp lệ
                return Reviews.Where(r => r.Rating.HasValue).Average(r => (double)r.Rating!.Value);
            }
        }

        // 2. GIÁ GỐC THẤP NHẤT (Giá bán thấp nhất của tùy chọn dung tích)
        // Đây là giá mặc định không nullable, trả về 0m nếu không có dung tích.
        [NotMapped]
        public decimal Price
        {
            get
            {
                // Lấy giá bán thấp nhất (GiaBan) từ VolumeOptions
                var minPriceOption = VolumeOptions?
                    .Where(v => v.Price.HasValue)
                    .OrderBy(v => v.Price)
                    .FirstOrDefault();

                // Trả về Price (decimal?) nếu có, nếu không thì 0m
                return minPriceOption?.Price ?? 0m;
            }
        }

        // 3. GIÁ ĐANG ÁP DỤNG (Giá sau chiết khấu hoặc khuyến mãi)
        // Đây là giá hiển thị trên giao diện, ưu tiên giá khuyến mãi nếu có.
        [NotMapped]
        public decimal? DiscountedPrice
        {
            get
            {
                // Lấy tùy chọn dung tích có giá thấp nhất
                var option = VolumeOptions?
                    .Where(v => v.Price.HasValue)
                    .OrderBy(v => v.Price)
                    .FirstOrDefault();

                // Nếu option.SalePrice có giá trị và nhỏ hơn Price gốc, sử dụng SalePrice             
                if (option != null && option.SalePrice.HasValue && option.SalePrice.Value < (option.Price ?? decimal.MaxValue))
                {
                    return option.SalePrice;
                }

                // Nếu không có SalePrice hoặc SalePrice lớn hơn giá gốc, không có chiết khấu/khuyến mãi cố định.
                return null;
            }
        }

        // 4. SỐ LƯỢNG TỒN TỔNG (Giữ nguyên)
        [NotMapped]
        public int Quantity
            => VolumeOptions?.Sum(v => v.Stock ?? 0) ?? 0;
    }
}
