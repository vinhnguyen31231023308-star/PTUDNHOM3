using System.Collections.Generic;

namespace EcommerceMVC.Models.ViewModels
{
    /// <summary>
    /// ViewModel tổng dữ liệu cho trang chủ.
    /// Mỗi property tương ứng với 1 khu vực (section) trên Index.cshtml.
    /// </summary>
    public class HomeViewModel
    {

        // Section: “SP hero” (các danh mục sản phẩm)
        public List<Product> HeroProducts { get; set; } = new();
        
        // Section: “SP nổi bật” (các danh mục sản phẩm)
        public List<Product> PromoProducts { get; set; } = new();
        
        // Section: “Khám phá theo nhu cầu” (các danh mục sản phẩm)
        public List<Category> Categories { get; set; } = new();

        // Section: “Sản phẩm bán chạy” – slider đầu tiên
        public List<Product> BestSellerProducts { get; set; } = new();

        // Section: “Sản phẩm mới” – slider thứ hai
        public List<Product> NewArrivalProducts { get; set; } = new();

        // Section: “Mã giảm giá / Ưu đãi có giới hạn”
        public List<Voucher> ActiveVouchers { get; set; } = new();

        // Section: “Tin tức / Blog mới nhất”
        public List<NewsArticle> LatestNews { get; set; } = new();
    }
}
