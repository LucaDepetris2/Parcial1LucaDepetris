using SQLite;

namespace Parcial1LucaDepetris.Models
{
    [Table("favorites")]
    public class FavoritePost
    {
        [PrimaryKey]
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public static FavoritePost FromPost(Post post) => new()
        {
            PostId = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body
        };
    }
}
