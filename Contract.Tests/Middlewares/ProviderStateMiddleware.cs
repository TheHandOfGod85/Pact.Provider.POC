using Microsoft.AspNetCore.Http;
using Provider.POC.Repositories;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Provider.Tests.Middlewares
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserRepository _repository;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next, IUserRepository repository)
        {
            _next = next;
            _repository = repository;
            _providerStates = new Dictionary<string, Action>
            {
                { "there is a user", UserExist },
            };
        }

        private void UserExist()
        {
            _repository.Get(1);
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/provider-states"))
            {
                await HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(string.Empty);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = string.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !string.IsNullOrEmpty(providerState.State))
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }
    }
}
