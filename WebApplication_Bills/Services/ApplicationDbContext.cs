using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication_Bills.Models;

namespace WebApplication_Bills.Services
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { 

        }
        public DbSet<WebApplication_Bills.Models.Bill> Bill { get; set; } = default!;
    }
}
