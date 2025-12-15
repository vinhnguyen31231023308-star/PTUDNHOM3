using System.Security.Cryptography;
using System.Text;
using HairNovaShop.Data;
using HairNovaShop.Models;
using HairNovaShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HairNovaShop.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private const int OTP_EXPIRY_MINUTES = 10;

    public AccountController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // GET: Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == model.Username);

        if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        if (!user.IsEmailVerified)
        {
            ModelState.AddModelError("", "Tài khoản chưa được xác thực email. Vui lòng kiểm tra email.");
            return View(model);
        }

        // Set session
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("FullName", user.FullName);
        HttpContext.Session.SetString("Role", user.Role.ToString());

        // Update last login
        user.LastLoginAt = DateTime.Now;
        await _context.SaveChangesAsync();

        if (model.RememberMe)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                IsEssential = true
            };
            Response.Cookies.Append("RememberMe", user.Id.ToString(), options);
        }

        // Redirect admin to admin panel, regular users to home
        if (user.Role == Role.Admin)
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction("Index", "Home");
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if username already exists
        if (await _context.Users.AnyAsync(u => u.Username == model.Email))
        {
            ModelState.AddModelError("Email", "Email này đã được sử dụng.");
            return View(model);
        }

        // Check if email already exists
        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email này đã được sử dụng.");
            return View(model);
        }

        // Check if phone already exists
        if (await _context.Users.AnyAsync(u => u.Phone == model.Phone))
        {
            ModelState.AddModelError("Phone", "Số điện thoại này đã được sử dụng.");
            return View(model);
        }

        // If OTP code is provided, verify it
        if (!string.IsNullOrEmpty(model.OTPCode))
        {
            var otp = await _context.OTPs
                .Where(o => o.Email == model.Email 
                         && o.Code == model.OTPCode 
                         && o.Type == OTPType.Registration 
                         && !o.IsUsed 
                         && o.ExpiresAt > DateTime.Now)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otp == null)
            {
                ModelState.AddModelError("OTPCode", "Mã OTP không hợp lệ hoặc đã hết hạn.");
                return View(model);
            }

            // Mark OTP as used
            otp.IsUsed = true;
            await _context.SaveChangesAsync();

            // Create user
            var user = new User
            {
                Username = model.Email,
                Email = model.Email,
                Phone = model.Phone,
                FullName = model.FullName,
                PasswordHash = HashPassword(model.Password),
                IsEmailVerified = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Auto login
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);

            return RedirectToAction("Index", "Home");
        }

        // If no OTP, send OTP email
        var otpCode = GenerateOTP();
        var newOtp = new OTP
        {
            Email = model.Email,
            Code = otpCode,
            Type = OTPType.Registration,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(OTP_EXPIRY_MINUTES)
        };

        _context.OTPs.Add(newOtp);
        await _context.SaveChangesAsync();

        try
        {
            await _emailService.SendOTPEmailAsync(model.Email, otpCode, OTPType.Registration);
            // Lưu thông tin vào session để dùng khi verify OTP
            HttpContext.Session.SetString("Register_Email", model.Email);
            HttpContext.Session.SetString("Register_FullName", model.FullName);
            HttpContext.Session.SetString("Register_Phone", model.Phone);
            HttpContext.Session.SetString("Register_Password", model.Password);
            // Redirect đến trang VerifyOTP
            return RedirectToAction("VerifyOTP", new { email = model.Email, type = "Registration" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Lỗi gửi email: {ex.Message}");
            return View(model);
        }
    }

    // GET: Account/ForgotPassword
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // POST: Account/ForgotPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Find user by email or phone
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == model.EmailOrPhone || u.Phone == model.EmailOrPhone);

        if (user == null)
        {
            ModelState.AddModelError("EmailOrPhone", "Không tìm thấy tài khoản với thông tin này.");
            return View(model);
        }

        // If OTP and new password provided, reset password
        if (!string.IsNullOrEmpty(model.OTPCode) && !string.IsNullOrEmpty(model.NewPassword))
        {
            var otp = await _context.OTPs
                .Where(o => o.Email == user.Email 
                         && o.Code == model.OTPCode 
                         && o.Type == OTPType.ForgotPassword 
                         && !o.IsUsed 
                         && o.ExpiresAt > DateTime.Now)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otp == null)
            {
                ModelState.AddModelError("OTPCode", "Mã OTP không hợp lệ hoặc đã hết hạn.");
                return View(model);
            }

            // Update password
            user.PasswordHash = HashPassword(model.NewPassword);
            otp.IsUsed = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Mật khẩu đã được đặt lại thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // Send OTP email
        var otpCode = GenerateOTP();
        var newOtp = new OTP
        {
            Email = user.Email,
            Code = otpCode,
            Type = OTPType.ForgotPassword,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(OTP_EXPIRY_MINUTES)
        };

        _context.OTPs.Add(newOtp);
        await _context.SaveChangesAsync();

        try
        {
            await _emailService.SendOTPEmailAsync(user.Email, otpCode, OTPType.ForgotPassword);
            // Redirect đến trang VerifyOTP
            return RedirectToAction("VerifyOTP", new { email = user.Email, type = "ForgotPassword" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Lỗi gửi email: {ex.Message}");
            return View(model);
        }
    }

    // GET: Account/VerifyOTP
    public IActionResult VerifyOTP(string email, string type)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(type))
        {
            return RedirectToAction("Register");
        }

        var otpType = type == "Registration" ? OTPType.Registration : OTPType.ForgotPassword;
        var viewModel = new VerifyOTPViewModel
        {
            Email = email,
            Type = otpType
        };

        // Lấy thông tin từ session nếu là đăng ký
        if (otpType == OTPType.Registration)
        {
            viewModel.FullName = HttpContext.Session.GetString("Register_FullName") ?? "";
            viewModel.Phone = HttpContext.Session.GetString("Register_Phone") ?? "";
            viewModel.Password = HttpContext.Session.GetString("Register_Password") ?? "";
            viewModel.ConfirmPassword = viewModel.Password;
        }

        return View(viewModel);
    }

    // POST: Account/VerifyOTP
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyOTP(VerifyOTPViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Verify OTP
        var otp = await _context.OTPs
            .Where(o => o.Email == model.Email
                     && o.Code == model.OTPCode
                     && o.Type == model.Type
                     && !o.IsUsed
                     && o.ExpiresAt > DateTime.Now)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (otp == null)
        {
            ModelState.AddModelError("OTPCode", "Mã OTP không hợp lệ hoặc đã hết hạn.");
            return View(model);
        }

        // Mark OTP as used
        otp.IsUsed = true;
        await _context.SaveChangesAsync();

        if (model.Type == OTPType.Registration)
        {
            // Lấy thông tin từ session
            var fullName = HttpContext.Session.GetString("Register_FullName") ?? model.FullName ?? "";
            var phone = HttpContext.Session.GetString("Register_Phone") ?? model.Phone ?? "";
            var password = HttpContext.Session.GetString("Register_Password") ?? model.Password ?? "";

            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email này đã được đăng ký.");
                return View(model);
            }

            // Create user
            var user = new User
            {
                Username = model.Email,
                Email = model.Email,
                Phone = phone,
                FullName = fullName,
                PasswordHash = HashPassword(password),
                IsEmailVerified = true,
                Role = Role.User, // Mặc định là User
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Xóa thông tin đăng ký khỏi session
            HttpContext.Session.Remove("Register_Email");
            HttpContext.Session.Remove("Register_FullName");
            HttpContext.Session.Remove("Register_Phone");
            HttpContext.Session.Remove("Register_Password");

            // Auto login
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);

            TempData["Success"] = "Đăng ký thành công!";
            // Redirect admin to admin panel after registration (though new users default to User role)
            if (user.Role == Role.Admin)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
        else // ForgotPassword
        {
            if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập mật khẩu mới.");
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(model);
            }

            // Update password
            user.PasswordHash = HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Mật khẩu đã được đặt lại thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
    }

    // GET: Account/ResendOTP
    public async Task<IActionResult> ResendOTP(string email, string type)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(type))
        {
            return RedirectToAction("Register");
        }

        var otpType = type == "Registration" ? OTPType.Registration : OTPType.ForgotPassword;
        var otpCode = GenerateOTP();
        var newOtp = new OTP
        {
            Email = email,
            Code = otpCode,
            Type = otpType,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(OTP_EXPIRY_MINUTES)
        };

        _context.OTPs.Add(newOtp);
        await _context.SaveChangesAsync();

        try
        {
            await _emailService.SendOTPEmailAsync(email, otpCode, otpType);
            TempData["Success"] = "Mã OTP mới đã được gửi đến email của bạn.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi gửi email: {ex.Message}";
        }

        // Get additional data for registration
        if (otpType == OTPType.Registration)
        {
            return RedirectToAction("VerifyOTP", new { email, type });
        }

        return RedirectToAction("VerifyOTP", new { email, type });
    }

    // GET: Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        Response.Cookies.Delete("RememberMe");
        return RedirectToAction("Login");
    }

    // Helper methods
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == passwordHash;
    }

    private string GenerateOTP()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
