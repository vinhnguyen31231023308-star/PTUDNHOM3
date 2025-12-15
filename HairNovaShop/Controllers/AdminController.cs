using System.Security.Cryptography;
using System.Text;
using HairNovaShop.Attributes;
using HairNovaShop.Data;
using HairNovaShop.Helpers;
using HairNovaShop.Models;
using Microsoft.AspNetCore.Mvc;
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
}
