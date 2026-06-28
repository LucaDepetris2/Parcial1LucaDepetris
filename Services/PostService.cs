using System.Net.Http.Json;
using Parcial1LucaDepetris.Models;

namespace Parcial1LucaDepetris.Services
{
    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://jsonplaceholder.typicode.com/posts";

        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync(ApiUrl);
            response.EnsureSuccessStatusCode();
            return await ParseResponseAsync(response);
        }

        private static async Task<List<Post>> ParseResponseAsync(HttpResponseMessage response)
        {
            var posts = await response.Content.ReadFromJsonAsync<List<Post>>();
            return posts ?? [];
        }
    }
}
