using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(NerdMonkey.Startup))]


namespace NerdMonkey
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<HomeTownDbContext>(config => {
                config.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NerdMonkey;Trusted_Connection=True;MultipleActiveResultSets=true");
            });
        }
    }
}