using System.Security.Cryptography;
using System.Text;
using HairNovaShop.Attributes;
using HairNovaShop.Data;
using HairNovaShop.Helpers;
using HairNovaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HairNovaShop.Controllers;

[AuthorizeAdmin]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin
    public IActionResult Index()
    {
        return View();
    }

    // GET: Admin/Users
    public async Task<IActionResult> Users()
    {
        var users = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
        return View(users);
    }

    // GET: Admin/SetAdmin/{id}
    public async Task<IActionResult> SetAdmin(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Role = Role.Admin;
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã cấp quyền Admin cho {user.FullName}";
        return RedirectToAction("Users");
    }

    // GET: Admin/RemoveAdmin/{id}
    public async Task<IActionResult> RemoveAdmin(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Role = Role.User;
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã gỡ quyền Admin của {user.FullName}";
        return RedirectToAction("Users");
    }

    // GET: Admin/Profile
    public async Task<IActionResult> Profile()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Admin/Profile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(User model)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        // Update user info
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.Phone = model.Phone;

        await _context.SaveChangesAsync();

        // Update session
        HttpContext.Session.SetString("FullName", user.FullName);

        TempData["Success"] = "Cập nhật thông tin thành công!";
        return RedirectToAction("Profile");
    }

    // GET: Admin/Users/Create
    public IActionResult CreateUser()
    {
        return View();
    }

    // POST: Admin/Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(User model, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Mật khẩu không được để trống.");
            return View(model);
        }

        // Check if username or email already exists
        if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
        {
            ModelState.AddModelError("", "Username hoặc Email đã tồn tại.");
            return View(model);
        }

        // Hash password
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var passwordHash = Convert.ToBase64String(hashedBytes);

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            Phone = model.Phone,
            FullName = model.FullName,
            PasswordHash = passwordHash,
            IsEmailVerified = true,
            Role = model.Role,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã tạo tài khoản {user.FullName} thành công!";
        return RedirectToAction("Users");
    }

    // GET: Admin/Users/Edit/{id}
    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Admin/Users/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int id, User model, string newPassword)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Check if username or email already exists (excluding current user)
        if (await _context.Users.AnyAsync(u => (u.Username == model.Username || u.Email == model.Email) && u.Id != id))
        {
            ModelState.AddModelError("", "Username hoặc Email đã tồn tại.");
            return View(user);
        }

        // Update user info
        user.Username = model.Username;
        user.Email = model.Email;
        user.Phone = model.Phone;
        user.FullName = model.FullName;
        user.Role = model.Role;

        // Update password if provided
        if (!string.IsNullOrEmpty(newPassword))
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            user.PasswordHash = Convert.ToBase64String(hashedBytes);
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã cập nhật thông tin {user.FullName} thành công!";
        return RedirectToAction("Users");
    }

    // POST: Admin/Users/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userName = user.FullName;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã xóa tài khoản {userName} thành công!";
        return RedirectToAction("Users");
    }

    // ==================== PRODUCTS MANAGEMENT ====================

    // GET: Admin/Products
    public async Task<IActionResult> Products()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return View(products);
    }

    // GET: Admin/Products/Create
    public async Task<IActionResult> CreateProduct()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    // POST: Admin/Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(Product model, IFormFile? mainImage, List<IFormFile>? images)
    {
        if (ModelState.IsValid)
        {
            // Convert main image to base64
            if (mainImage != null && mainImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await mainImage.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(imageBytes);
                    var contentType = mainImage.ContentType ?? "image/jpeg";
                    model.MainImage = $"data:{contentType};base64,{base64String}";
                }
            }

            // Convert additional images to base64
            if (images != null && images.Any())
            {
                var base64Images = new List<string>();
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            var imageBytes = memoryStream.ToArray();
                            var base64String = Convert.ToBase64String(imageBytes);
                            var contentType = image.ContentType ?? "image/jpeg";
                            base64Images.Add($"data:{contentType};base64,{base64String}");
                        }
                    }
                }
                if (base64Images.Any())
                {
                    model.Images = System.Text.Json.JsonSerializer.Serialize(base64Images);
                }
            }

            model.CreatedAt = DateTime.Now;
            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã tạo sản phẩm {model.Name} thành công!";
            return RedirectToAction("Products");
        }

        // Load categories again if ModelState is invalid
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
        return View(model);
    }

    // GET: Admin/Products/Edit/{id}
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // POST: Admin/Products/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, Product model, IFormFile? mainImage, List<IFormFile>? images)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Convert new main image to base64 if provided
            if (mainImage != null && mainImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await mainImage.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(imageBytes);
                    var contentType = mainImage.ContentType ?? "image/jpeg";
                    product.MainImage = $"data:{contentType};base64,{base64String}";
                }
            }

            // Convert additional images to base64 if provided
            if (images != null && images.Any(i => i.Length > 0))
            {
                var base64Images = new List<string>();
                
                // Keep existing images (they should already be base64)
                if (!string.IsNullOrEmpty(product.Images))
                {
                    try
                    {
                        var existingImages = System.Text.Json.JsonSerializer.Deserialize<List<string>>(product.Images);
                        if (existingImages != null)
                        {
                            base64Images.AddRange(existingImages);
                        }
                    }
                    catch { }
                }

                // Add new images as base64
                foreach (var image in images.Where(i => i.Length > 0))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        var base64String = Convert.ToBase64String(imageBytes);
                        var contentType = image.ContentType ?? "image/jpeg";
                        base64Images.Add($"data:{contentType};base64,{base64String}");
                    }
                }

                product.Images = System.Text.Json.JsonSerializer.Serialize(base64Images);
            }

            // Update other fields
            product.Name = model.Name;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.DetailedDescription = model.DetailedDescription;
            product.Price = model.Price;
            product.OriginalPrice = model.OriginalPrice;
            product.SKU = model.SKU;
            product.Tags = model.Tags;
            product.Brand = model.Brand;
            product.Origin = model.Origin;
            product.Capacity = model.Capacity;
            product.ExpiryDate = model.ExpiryDate;
            product.Stock = model.Stock;
            product.Rating = model.Rating;
            product.ReviewCount = model.ReviewCount;
            product.IsActive = model.IsActive;
            product.IsFeatured = model.IsFeatured;
            product.IsNew = model.IsNew;
            product.OnSale = model.OnSale;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã cập nhật sản phẩm {product.Name} thành công!";
            return RedirectToAction("Products");
        }

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // POST: Admin/Products/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // No need to delete files since images are stored as base64 in database
        var productName = product.Name;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã xóa sản phẩm {productName} thành công!";
        return RedirectToAction("Products");
    }

    // ==================== CATEGORIES MANAGEMENT ====================

    // GET: Admin/Categories
    public async Task<IActionResult> Categories()
    {
        var categories = await _context.Categories
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        return View(categories);
    }

    // GET: Admin/Categories/Create
    public IActionResult CreateCategory()
    {
        return View();
    }

    // POST: Admin/Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(Category model)
    {
        if (ModelState.IsValid)
        {
            model.CreatedAt = DateTime.Now;
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã tạo danh mục {model.Name} thành công!";
            return RedirectToAction("Categories");
        }

        return View(model);
    }

    // GET: Admin/Categories/Edit/{id}
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Admin/Categories/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(int id, Category model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            category.Name = model.Name;
            category.Description = model.Description;
            category.IsActive = model.IsActive;
            category.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã cập nhật danh mục {category.Name} thành công!";
            return RedirectToAction("Categories");
        }

        return View(model);
    }

    // POST: Admin/Categories/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        // Check if category has products
        var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProducts)
        {
            TempData["Error"] = $"Không thể xóa danh mục {category.Name} vì còn sản phẩm đang sử dụng danh mục này!";
            return RedirectToAction("Categories");
        }

        var categoryName = category.Name;
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Đã xóa danh mục {categoryName} thành công!";
        return RedirectToAction("Categories");
    }
}
