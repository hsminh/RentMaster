using Microsoft.EntityFrameworkCore;
using RentMaster.Accounts.Models;

namespace RentMaster.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<consumer> Consumers { get; set; }
    }
}