using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NerdMonkey
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<HomeTownDbContext>
    {
        public HomeTownDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Design.json")
                .Build();
            var connStr = config.GetConnectionString("Default");
            var optionsBuilder = new DbContextOptionsBuilder<HomeTownDbContext>();
            optionsBuilder.UseSqlServer(connStr);
            return new HomeTownDbContext(optionsBuilder.Options);
        }
    }
}