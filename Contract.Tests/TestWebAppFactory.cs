using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Provider.POC.Repositories;
using Provider.Tests.Middlewares;

namespace Provider.Tests
{
    public class TestWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Add any test-specific services here
                services.AddScoped<IUserRepository, UserRepository>();
            });

            builder.Configure(app =>
            {
                app.UseMiddleware<ProviderStateMiddleware>();
            });

            base.ConfigureWebHost(builder);
        }
    }
}
