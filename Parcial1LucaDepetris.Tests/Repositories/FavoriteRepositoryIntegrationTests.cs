using SQLite;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;

namespace Parcial1LucaDepetris.Tests.Repositories
{
    // xUnit crea una nueva instancia por cada test, garantizando aislamiento
    public class FavoriteRepositoryIntegrationTests : IDisposable
    {
        private readonly SQLiteAsyncConnection _connection;

        public FavoriteRepositoryIntegrationTests()
        {
            _connection = new SQLiteAsyncConnection(":memory:");
        }

        private FavoriteRepository CreateRepository() => new(_connection);

        public void Dispose()
        {
            _connection.CloseAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Guardar_LuegoObtenerTodos_DevuelveElFavoriteGuardado()
        {
            var repo = CreateRepository();
            var favorite = new FavoritePost { PostId = 1, UserId = 1, Title = "Test", Body = "Cuerpo" };

            await repo.SaveAsync(favorite);
            var resultados = await repo.GetAllAsync();

            Assert.Single(resultados);
            Assert.Equal("Test", resultados[0].Title);
        }

        [Fact]
        public async Task Eliminar_CuandoElFavoriteExiste_LoRemueveDeLaBaseDeDatos()
        {
            var repo = CreateRepository();
            var favorite = new FavoritePost { PostId = 1, UserId = 1, Title = "Test", Body = "Cuerpo" };
            await repo.SaveAsync(favorite);

            await repo.DeleteAsync(1);
            var resultados = await repo.GetAllAsync();

            Assert.Empty(resultados);
        }

        [Fact]
        public async Task Guardar_ConIdDuplicado_ActualizaElRegistroExistente()
        {
            var repo = CreateRepository();
            var original = new FavoritePost { PostId = 1, UserId = 1, Title = "Original", Body = "Cuerpo" };
            var actualizado = new FavoritePost { PostId = 1, UserId = 1, Title = "Actualizado", Body = "Cuerpo" };

            await repo.SaveAsync(original);
            await repo.SaveAsync(actualizado);
            var resultados = await repo.GetAllAsync();

            Assert.Single(resultados);
            Assert.Equal("Actualizado", resultados[0].Title);
        }

        [Fact]
        public async Task ObtenerTodos_CuandoEstaVacio_DevuelveListaVacia()
        {
            var repo = CreateRepository();

            var resultados = await repo.GetAllAsync();

            Assert.Empty(resultados);
        }

        [Fact]
        public async Task Guardar_MultiplesRegistros_LosDevuelveTodos()
        {
            var repo = CreateRepository();
            await repo.SaveAsync(new FavoritePost { PostId = 1, UserId = 1, Title = "A", Body = "B" });
            await repo.SaveAsync(new FavoritePost { PostId = 2, UserId = 1, Title = "C", Body = "D" });

            var resultados = await repo.GetAllAsync();

            Assert.Equal(2, resultados.Count);
        }
    }
}
