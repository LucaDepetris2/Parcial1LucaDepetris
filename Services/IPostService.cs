using Parcial1LucaDepetris.Models;

namespace Parcial1LucaDepetris.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();
    }
}
