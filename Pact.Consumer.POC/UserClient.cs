using System.Net.Http.Json;

namespace Consumer.POC
{
    public class UserClient
    {
        private readonly Uri baseUri;

        public UserClient(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public async Task<User> GetUser(int id)
        {
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    var response = await client.GetAsync($"/user?id={id}");
                    response.EnsureSuccessStatusCode();
                    return (await response.Content.ReadFromJsonAsync<User>())!;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to the User API.", ex);
                }
            }
        }
    }
}
