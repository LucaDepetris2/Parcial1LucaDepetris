using Moq;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;
using Parcial1LucaDepetris.ViewModels;

namespace Parcial1LucaDepetris.Tests.ViewModels
{
    public class DetailViewModelTests
    {
        private readonly Mock<IFavoriteRepository> _repositoryMock = new();

        [Fact]
        public async Task GuardarFavorito_CuandoPostEstaAsignado_LlamaAlRepositorio()
        {
            var vm = new DetailViewModel(_repositoryMock.Object)
            {
                Post = new Post { Id = 5, UserId = 1, Title = "Título", Body = "Cuerpo" }
            };

            await vm.SaveFavoriteCommand.ExecuteAsync(null);

            _repositoryMock.Verify(r => r.SaveAsync(It.Is<FavoritePost>(f => f.PostId == 5)), Times.Once);
        }

        [Fact]
        public async Task GuardarFavorito_CuandoTieneExito_EstableceMensajeDeEstado()
        {
            _repositoryMock.Setup(r => r.SaveAsync(It.IsAny<FavoritePost>())).Returns(Task.CompletedTask);
            var vm = new DetailViewModel(_repositoryMock.Object)
            {
                Post = new Post { Id = 1, UserId = 1, Title = "T", Body = "B" }
            };

            await vm.SaveFavoriteCommand.ExecuteAsync(null);

            Assert.NotEmpty(vm.SaveStatus);
        }

        [Fact]
        public async Task GuardarFavorito_CuandoPostEsNulo_NoLlamaAlRepositorio()
        {
            var vm = new DetailViewModel(_repositoryMock.Object) { Post = null };

            await vm.SaveFavoriteCommand.ExecuteAsync(null);

            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<FavoritePost>()), Times.Never);
        }

        [Fact]
        public async Task GuardarFavorito_CuandoElRepositorioLanzaExcepcion_EstableceMensajeDeError()
        {
            _repositoryMock.Setup(r => r.SaveAsync(It.IsAny<FavoritePost>()))
                .ThrowsAsync(new InvalidOperationException("Error de base de datos"));
            var vm = new DetailViewModel(_repositoryMock.Object)
            {
                Post = new Post { Id = 1, UserId = 1, Title = "T", Body = "B" }
            };

            await vm.SaveFavoriteCommand.ExecuteAsync(null);

            Assert.Contains("Error", vm.SaveStatus);
        }
    }
}
