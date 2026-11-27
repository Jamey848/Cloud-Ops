using Recommendations_engine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("game_genres")]
    public class GameGenre
    {
        [Key]
        [Column("gg_id", TypeName = "text")]
        public string GgId { get; set; } = null!;

        [Column("appid", TypeName = "bigint")]
        public long? AppId { get; set; }

        [Column("genre_id", TypeName = "text")]
        public string? GenreId { get; set; }

        public Genre Linked_genre { get; set; }
        public game? game { get; set; }
    }

}
