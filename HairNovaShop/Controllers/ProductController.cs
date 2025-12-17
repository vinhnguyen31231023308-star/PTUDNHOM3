using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairNovaShop.Data;
using HairNovaShop.Models;

namespace HairNovaShop.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null || !product.IsActive)
        {
            return NotFound();
        }

        // Get related products (same category)
        var relatedProducts = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id != id && p.CategoryId == product.CategoryId && p.IsActive)
            .OrderByDescending(p => p.Rating)
            .Take(4)
            .ToListAsync();

        ViewBag.RelatedProducts = relatedProducts;

        return View(product);
    }
}
