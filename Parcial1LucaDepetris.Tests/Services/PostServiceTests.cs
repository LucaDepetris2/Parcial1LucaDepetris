using System.Net;
using System.Text.Json;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;

namespace Parcial1LucaDepetris.Tests.Services
{
    public class PostServiceTests
    {
        private static HttpClient CreateClient(HttpStatusCode statusCode, string? content = null)
        {
            var handler = new FakeHttpMessageHandler(statusCode, content);
            return new HttpClient(handler) { BaseAddress = new Uri("https://example.com") };
        }

        [Fact]
        public async Task ObtenerPosts_CuandoLaApiRespondeOk_DevuelvePosts()
        {
            var posts = new List<Post>
            {
                new() { Id = 1, UserId = 1, Title = "Título 1", Body = "Cuerpo" }
            };
            var json = JsonSerializer.Serialize(posts);
            var client = CreateClient(HttpStatusCode.OK, json);
            var service = new PostService(client);

            var result = await service.GetPostsAsync();

            Assert.Single(result);
            Assert.Equal("Título 1", result[0].Title);
        }

        [Fact]
        public async Task ObtenerPosts_CuandoLaApiResponde404_LanzaHttpRequestException()
        {
            var client = CreateClient(HttpStatusCode.NotFound);
            var service = new PostService(client);

            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetPostsAsync());
        }

        [Fact]
        public async Task ObtenerPosts_CuandoLaApiRespondeArrayVacio_DevuelveListaVacia()
        {
            var client = CreateClient(HttpStatusCode.OK, "[]");
            var service = new PostService(client);

            var result = await service.GetPostsAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task ObtenerPosts_CuandoLaApiResponde500_LanzaHttpRequestException()
        {
            var client = CreateClient(HttpStatusCode.InternalServerError);
            var service = new PostService(client);

            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetPostsAsync());
        }
    }

    internal sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string? _content;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string? content = null)
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode);
            if (_content is not null)
                response.Content = new StringContent(_content, System.Text.Encoding.UTF8, "application/json");
            return Task.FromResult(response);
        }
    }
}
