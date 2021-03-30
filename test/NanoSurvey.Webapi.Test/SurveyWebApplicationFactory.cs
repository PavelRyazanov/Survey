using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoSurvey.Data;

namespace NanoSurvey.Webapi.Test
{
    public class SurveyWebApplicationFactory
        : WebApplicationFactory<NanoSurvey.Webapi.Startup>
    {
        private static bool dataSeeded = false;

        private static object lockObject = new object();

        protected override void ConfigureWebHost(IWebHostBuilder webhostBuilder)
        {
            lock(lockObject)
            {
                if (dataSeeded)
                    return;

                webhostBuilder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SurveyDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddDbContext<SurveyDbContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));

                    var serviceProviderBuilder = services.BuildServiceProvider();
                    using (var scope = serviceProviderBuilder.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var dbContext = scopedServices.GetRequiredService<SurveyDbContext>();

                        dbContext.Database.EnsureCreated();

                        Seed.SeedDb(dbContext);

                        dbContext.SaveChanges();
                    }
                });

                dataSeeded = true;
            }
            
        }
    }
}