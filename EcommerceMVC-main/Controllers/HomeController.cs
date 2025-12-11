using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceMVC.Data;
using EcommerceMVC.Models;
using EcommerceMVC.Models.ViewModels;

namespace EcommerceMVC.Controllers
{
    /// <summary>
    /// Controller xử lý trang chủ HairNova.
    /// Toàn bộ dữ liệu hiển thị trên Home được gom trong action Index().
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --------------------------------------------------------------------
        // GET: /
        // Mục tiêu: 
        //   - Chuẩn bị dữ liệu cho từng section trên file Views/Home/Index.cshtml
        //   - Không xử lý logic phức tạp, chỉ query mức cơ bản để tránh lỗi.
        // --------------------------------------------------------------------
        public async Task<IActionResult> Index(List<NewsArticle> newsArticles)
        {
            var vm = new HomeViewModel();

            // ================================================================
            // 1. HERO / BANNER
            //    - Dùng các sản phẩm mới nhất làm “gợi ý” cho cụm card quanh điện thoại
            //    - Trên HTML: phần .banner, .card-product, .card-rating, ...
            // ================================================================
            vm.HeroProducts = await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.CreatedAt)
                .Take(3)
                .ToListAsync();

            // ================================================================
            // 2. PROMO SECTION
            //    - 2 khối promo bên dưới banner chính.
            //    - Ở đây tạm lấy 2 sản phẩm bất kỳ (sau này có cờ “NoiBat” thì lọc thêm).
            // ================================================================
            vm.PromoProducts = await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.IsFeatured)   // Nếu cột NoiBat là BIT trong DB
                .ThenByDescending(p => p.CreatedAt)
                .Take(2)
                .ToListAsync();

            // ================================================================
            // 3. BEST SELLERS – “Sản phẩm bán chạy”
            //    - Dùng danh sách sản phẩm sắp xếp theo cột DaBan (nếu có).
            //    - Hiển thị trên slider có id="productTrack".
            // ================================================================
            vm.BestSellerProducts = await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.SoldQuantity)
                .ThenByDescending(p => p.CreatedAt)
                .Take(12)
                .ToListAsync();

            // ================================================================
            // 4. DEALS & COUPONS
            //    - Map với section .deals-section: 4 thẻ mã giảm giá.
            //    - Chọn các voucher đang còn hiệu lực (TrangThai = true, trong khoảng thời gian).
            // ================================================================
            var now = DateTime.Now;

            vm.ActiveVouchers = await _context.Vouchers
                .Where(v =>
                    v.IsActive == true &&
                    (v.StartDate == null || v.StartDate <= now) &&
                    (v.EndDate == null || v.EndDate >= now))
                .OrderBy(v => v.EndDate)
                .Take(8)
                .ToListAsync();

            // ================================================================
            // 5. NEW ARRIVALS – “Hàng mới về”
            //    - Lấy các sản phẩm mới nhất (khác với Best Sellers).
            //    - Dùng cho lưới ở section .arrival-section.
            // ================================================================
            vm.NewArrivalProducts = await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.CreatedAt)
                .Take(6)
                .ToListAsync();

            // ================================================================
            // 6. TESTIMONIALS – đánh giá khách hàng
            //    - Map với slider Swiper “Khách hàng nói gì?”.
            //    - Chỉ lấy các review đã duyệt (DaDuyet = true).
            // ================================================================
            //vm.Testimonials = await _context.Reviews
            //    .Include(r => r.Customer)            // nếu Review có navigation Customer
            //    .Where(r => r.IsApproved == true)
            //    .OrderByDescending(r => r.CreatedAt)
            //    .Take(10)
            //    .ToListAsync();

            // ================================================================
            // 7. NEWS / BLOG – Tin tức & bài viết
            //    - Map với section .news-section (1 bài lớn + 2 bài nhỏ).
            // ================================================================
            List<NewsArticle> NewsArticles = await _context.NewsArticles
                .OrderByDescending(n => n.PublishedAt)
                .Take(5)
                .ToListAsync();
            vm.LatestNews = NewsArticles;

            // ================================================================
            // 8. CATEGORY SECTION – “Khám phá theo nhu cầu”
            //    - Dùng danh mục sản phẩm chính để hiển thị card dạng shape.
            // ================================================================
            vm.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            // ================================================================
            // 9. PARTNERS SECTION – Logo thương hiệu / đối tác
            //    - Map với slider logo ở section .partners-section.
            // ================================================================
            //vm.Brands = await _context.Brands
            //  .OrderBy(b => b.Name)
            //  .ToListAsync();

            // Cuối cùng trả về View cùng ViewModel
            return View(vm);
        }
    }
}
