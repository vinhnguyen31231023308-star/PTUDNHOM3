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
}
