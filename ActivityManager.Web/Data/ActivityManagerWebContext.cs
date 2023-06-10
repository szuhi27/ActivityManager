using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActivityManager.Web.Models;

namespace ActivityManager.Web.Data
{
    public class ActivityManagerWebContext : DbContext
    {
        public ActivityManagerWebContext (DbContextOptions<ActivityManagerWebContext> options)
            : base(options)
        {
        }

        public DbSet<ActivityType> ActivityType { get; set; } = default!;

        public DbSet<Activity> Activity { get; set; } = default!;
    }
}
