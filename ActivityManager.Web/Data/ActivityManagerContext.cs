using ActivityManager.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityManager.Web.Data
{
    public class ActivityManagerContext : DbContext
    {
        public DbSet<ActivityType> ActivityTypes { get; set; } = null!;
        public DbSet<Activity> Activities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }
    }
}
