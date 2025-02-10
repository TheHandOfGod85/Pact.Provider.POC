using Contract.Tests.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Pact.Provider.POC.Repositories;

namespace Contract.Tests
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
