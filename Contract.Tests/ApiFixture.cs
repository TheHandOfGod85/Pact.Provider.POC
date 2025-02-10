using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Contract.Tests
{
    public class ApiFixture : IDisposable
    {
        private readonly IHost server;
        public Uri ServerUri { get; }

        public ApiFixture()
        {
            ServerUri = new Uri("http://localhost:5000");
            server = Host.CreateDefaultBuilder()
                         .ConfigureWebHostDefaults(webBuilder =>
                         {
                             webBuilder.UseUrls(ServerUri.ToString());
                             webBuilder.UseStartup<Program>();
                         })
                         .Build();
            server.Start();
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
