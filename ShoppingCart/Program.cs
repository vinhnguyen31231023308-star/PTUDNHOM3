using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(connectionString));

            builder.Services.AddScoped<ProductService, ProductServiceImpl>();
            builder.Services.AddScoped<CartService, CartServiceImpl>();
            var app = builder.Build();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}"
                );

            app.Run();
        }
    }
}
