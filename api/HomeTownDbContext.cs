using Microsoft.EntityFrameworkCore;
using NerdMonkey.Models;

namespace NerdMonkey
{
    public class HomeTownDbContext : DbContext
    {
        public HomeTownDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }





        public DbSet<UserModel> Users { get; set; }
    }
}