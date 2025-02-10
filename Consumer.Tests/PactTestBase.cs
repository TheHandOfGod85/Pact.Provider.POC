using Consumer.POC;
using PactNet;
using PactNet.Output.Xunit;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace Consumer.Tests
{
    public class PactTestBase
    {
        private readonly IPactBuilderV4 pactBuilder;

        public PactTestBase(ITestOutputHelper output)
        {
            var config = new PactConfig
            {
                PactDir = "../../../pacts/",
                Outputters =
                [
                    new XunitOutput(output)
                ],

                DefaultJsonSettings = new JsonSerializerOptions(),
                LogLevel = PactLogLevel.Debug,
            };

            IPactV4 pact = Pact.V4("DotNet-consumer", "DotNet-API", config);
            pactBuilder = pact.WithHttpInteractions(5000);
        }

        [Fact]
        public async Task GetUserWhenUserExistsReturnsUser()
        {
            pactBuilder
            .UponReceiving("A GET request to retrieve the user")
                .Given("There is a user with id 1")
                .WithRequest(HttpMethod.Get, "/user")
                .WithQuery("id", "1")
                .WithHeader("Accept", "application/json")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithJsonBody(new
                {
                    id = 1,
                    name = "John Doe"
                });

            await pactBuilder.VerifyAsync(async ctx =>
            {
                var client = new UserClient(ctx.MockServerUri);
                var user = await client.GetUser(1);

                Assert.Equal(1, user.Id);
                Assert.Equal("John Doe", user.Name);
            });

            using var client = new HttpClient();
            var content = new StringContent(File.ReadAllText("../../../pacts/DotNet-consumer-DotNet-API.json"), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("http://localhost:9292/pacts/provider/DotNet-API/consumer/DotNet-Consumer/version/1.0.0", content);
            Console.WriteLine(response.StatusCode);
        }
    }
}
