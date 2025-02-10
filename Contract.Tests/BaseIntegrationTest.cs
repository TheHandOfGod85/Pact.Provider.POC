using Microsoft.Extensions.DependencyInjection;
using Pact.Provider.POC.Repositories;

namespace Contract.Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<TestWebAppFactory>
    {
        private readonly IServiceScope _serviceScope;
        protected readonly IUserRepository _userRepository;
        protected string _pactServiceUri = "http://127.0.0.1:5000";

        protected BaseIntegrationTest(TestWebAppFactory factory)
        {
            _serviceScope = factory.Services.CreateScope();
            _userRepository = _serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
        }
    }
}
