using Moq;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;
using Parcial1LucaDepetris.ViewModels;

namespace Parcial1LucaDepetris.Tests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly Mock<IPostService> _postServiceMock = new();
        private readonly Mock<INavigationService> _navigationMock = new();

        private MainViewModel CreateViewModel() =>
            new(_postServiceMock.Object, _navigationMock.Object);

        [Fact]
        public async Task LoadPosts_CuandoElServicioDevuelveDatos_RellenaLaColeccion()
        {
            var posts = new List<Post>
            {
                new() { Id = 1, UserId = 1, Title = "Post 1", Body = "Cuerpo 1" },
                new() { Id = 2, UserId = 1, Title = "Post 2", Body = "Cuerpo 2" }
            };
            _postServiceMock.Setup(s => s.GetPostsAsync()).ReturnsAsync(posts);

            var vm = CreateViewModel();
            await vm.LoadPostsCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.Posts.Count);
            Assert.Contains(vm.Posts, p => p.Title == "Post 1");
        }

        [Fact]
        public async Task LoadPosts_CuandoElServicioDevuelveDatos_EstableceMensajeDeExito()
        {
            _postServiceMock.Setup(s => s.GetPostsAsync())
                .ReturnsAsync([new Post { Id = 1, Title = "T", Body = "B" }]);

            var vm = CreateViewModel();
            await vm.LoadPostsCommand.ExecuteAsync(null);

            Assert.Contains("1", vm.StatusMessage);
        }

        [Fact]
        public async Task LoadPosts_CuandoHayErrorDeRed_EstableceMensajeDeError()
        {
            _postServiceMock.Setup(s => s.GetPostsAsync())
                .ThrowsAsync(new HttpRequestException("Sin conexión"));

            var vm = CreateViewModel();
            await vm.LoadPostsCommand.ExecuteAsync(null);

            Assert.Contains("red", vm.StatusMessage, StringComparison.OrdinalIgnoreCase);
            Assert.Empty(vm.Posts);
        }

        [Fact]
        public async Task LoadPosts_DuranteLaCarga_IsLoadingEsTrue()
        {
            var tcs = new TaskCompletionSource<List<Post>>();
            _postServiceMock.Setup(s => s.GetPostsAsync()).Returns(tcs.Task);

            var vm = CreateViewModel();
            var loadTask = vm.LoadPostsCommand.ExecuteAsync(null);

            Assert.True(vm.IsLoading);
            Assert.False(vm.IsNotLoading);

            tcs.SetResult([]);
            await loadTask;

            Assert.False(vm.IsLoading);
        }

        [Fact]
        public async Task LoadPosts_CuandoFinalizaConExito_IsLoadingEsFalse()
        {
            _postServiceMock.Setup(s => s.GetPostsAsync()).ReturnsAsync([]);

            var vm = CreateViewModel();
            await vm.LoadPostsCommand.ExecuteAsync(null);

            Assert.False(vm.IsLoading);
            Assert.True(vm.IsNotLoading);
        }
    }
}
