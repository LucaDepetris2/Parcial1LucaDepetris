using SQLite;
using Parcial1LucaDepetris.Models;

namespace Parcial1LucaDepetris.Services
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly SQLiteAsyncConnection _db;
        private readonly Task _initTask;

        public FavoriteRepository(SQLiteAsyncConnection db)
        {
            _db = db;
            _initTask = db.CreateTableAsync<FavoritePost>();
        }

        private Task EnsureInitAsync() => _initTask;

        public async Task SaveAsync(FavoritePost favorite)
        {
            await EnsureInitAsync();
            await _db.InsertOrReplaceAsync(favorite);
        }

        public async Task<List<FavoritePost>> GetAllAsync()
        {
            await EnsureInitAsync();
            return await _db.Table<FavoritePost>().ToListAsync();
        }

        public async Task DeleteAsync(int postId)
        {
            await EnsureInitAsync();
            await _db.DeleteAsync<FavoritePost>(postId);
        }
    }
}
