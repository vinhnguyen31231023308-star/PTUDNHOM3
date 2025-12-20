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

            // Handle StockByCapacity from form
            var stockByCapacityJson = Request.Form["StockByCapacity"].ToString();
            if (!string.IsNullOrEmpty(stockByCapacityJson))
            {
                model.StockByCapacity = stockByCapacityJson;
                // Calculate total stock from variants
                try
                {
                    var variants = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stockByCapacityJson);
                    if (variants != null)
                    {
                        model.Stock = variants.Sum(v => v.ContainsKey("Stock") && v["Stock"] != null 
                            ? int.Parse(v["Stock"].ToString() ?? "0") 
                            : 0);
                    }
                }
                catch { }
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
            product.ExpiryDate = model.ExpiryDate;
            
            // Handle StockByCapacity from form
            var stockByCapacityJson = Request.Form["StockByCapacity"].ToString();
            if (!string.IsNullOrEmpty(stockByCapacityJson))
            {
                product.StockByCapacity = stockByCapacityJson;
                // Calculate total stock from variants
                try
                {
                    var variants = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stockByCapacityJson);
                    if (variants != null)
                    {
                        product.Stock = variants.Sum(v => v.ContainsKey("Stock") && v["Stock"] != null 
                            ? int.Parse(v["Stock"].ToString() ?? "0") 
                            : 0);
                    }
                }
                catch { }
            }
            else
            {
                product.Stock = model.Stock;
            }
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

    // GET: Admin/Orders
    public async Task<IActionResult> Orders()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        return View(orders);
    }

    // GET: Admin/Orders/Details/{id}
    public async Task<IActionResult> OrderDetails(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Admin/Orders/UpdateStatus/{id}
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
        }

        // Validate status transition
        var validTransition = IsValidStatusTransition(order.Status, status);
        if (!validTransition.isValid)
        {
            return Json(new { success = false, message = validTransition.message });
        }

        var oldStatus = order.Status;
        order.Status = status;
        order.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = $"Đã cập nhật trạng thái đơn hàng {order.OrderCode} từ \"{GetStatusName(oldStatus)}\" thành \"{GetStatusName(status)}\"",
            newStatus = status,
            newStatusName = GetStatusName(status),
            newStatusClass = GetStatusClass(status)
        });
    }

    // Validate status transition logic
    private (bool isValid, string message) IsValidStatusTransition(string currentStatus, string newStatus)
    {
        // Same status - no change needed
        if (currentStatus == newStatus)
        {
            return (false, "Trạng thái không thay đổi");
        }

        // Completed orders cannot be changed
        if (currentStatus == "completed")
        {
            return (false, "Đơn hàng đã hoàn thành không thể thay đổi trạng thái");
        }

        // Cancelled orders cannot be changed
        if (currentStatus == "cancelled")
        {
            return (false, "Đơn hàng đã hủy không thể thay đổi trạng thái");
        }

        // Allow cancellation from any active status
        if (newStatus == "cancelled")
        {
            return (true, "");
        }

        // Define valid transitions
        var validTransitions = new Dictionary<string, List<string>>
        {
            { "pending", new List<string> { "confirmed", "cancelled" } },
            { "confirmed", new List<string> { "shipping", "cancelled" } },
            { "shipping", new List<string> { "completed", "cancelled" } }
        };

        if (validTransitions.TryGetValue(currentStatus, out var allowedStatuses))
        {
            if (allowedStatuses.Contains(newStatus))
            {
                return (true, "");
            }
            else
            {
                var currentName = GetStatusName(currentStatus);
                var newName = GetStatusName(newStatus);
                return (false, $"Không thể chuyển từ \"{currentName}\" sang \"{newName}\". Vui lòng thực hiện theo đúng quy trình!");
            }
        }

        return (false, "Trạng thái không hợp lệ");
    }

    private string GetStatusClass(string status)
    {
        return status switch
        {
            "pending" => "bg-warning",
            "confirmed" => "bg-info",
            "shipping" => "bg-primary",
            "completed" => "bg-success",
            "cancelled" => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private string GetStatusName(string status)
    {
        return status switch
        {
            "pending" => "Chờ xác nhận",
            "confirmed" => "Đã xác nhận",
            "shipping" => "Đang giao hàng",
            "completed" => "Hoàn thành",
            "cancelled" => "Đã hủy",
            _ => status
        };
    }
}
