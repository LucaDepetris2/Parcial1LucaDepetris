using Parcial1LucaDepetris.Models;

namespace Parcial1LucaDepetris.Services
{
    public interface IFavoriteRepository
    {
        Task SaveAsync(FavoritePost favorite);
        Task<List<FavoritePost>> GetAllAsync();
        Task DeleteAsync(int postId);
    }
}
