using HairNovaShop.Models;
using Microsoft.EntityFrameworkCore;

namespace HairNovaShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<OTP> OTPs { get; set; }
}
