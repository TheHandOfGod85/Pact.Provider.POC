using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Provider.POC;
using Provider.Tests.Middlewares;
using Xunit.Abstractions;

namespace Provider.Tests
{
    public class UserTests
    {
        private readonly ITestOutputHelper _output;
        private readonly string _pactServiceUri = "http://localhost:5000";
        public UserTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        [Fact]
        public void GetUserContractTest()
        {
            using var app = Startup.WebApp();
            app.Urls.Add(_pactServiceUri);
            app.UseMiddleware<ProviderStateMiddleware>();
            app.Start();

            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XunitOutput(_output)
                },
                LogLevel = PactLogLevel.Debug,
            };

            var pactBrokerUrl = "http://127.0.0.1:8000";
            var pactBrokerUsername = "pact_workshop";
            var pactBrokerPassword = "pact_workshop";


            IPactVerifier pactVerifier = new PactVerifier("DotNet-API", config);

            pactVerifier
                .WithHttpEndpoint(new Uri(_pactServiceUri))
                .WithPactBrokerSource(new Uri(pactBrokerUrl), options =>
                {
                    options.BasicAuthentication(pactBrokerUsername, pactBrokerPassword);
                    options.PublishResults("1.0.0");
                })
                .WithProviderStateUrl(new Uri($"{_pactServiceUri}/provider-states"))
                .Verify();
        }
    }
}
