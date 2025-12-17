using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairNovaShop.Models;
using HairNovaShop.Data;

namespace HairNovaShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var featuredProducts = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsFeatured && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(8)
            .ToListAsync();

        var newProducts = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsNew && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(8)
            .ToListAsync();

        var onSaleProducts = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.OnSale && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(8)
            .ToListAsync();

        ViewBag.FeaturedProducts = featuredProducts;
        ViewBag.NewProducts = newProducts;
        ViewBag.OnSaleProducts = onSaleProducts;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
