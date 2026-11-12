using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("genres")]
    public class Genre
    {
        [Key]
        [Column("genre_id", TypeName = "text")]
        public string GenreId { get; set; } = null!;

        [Column("genre_name", TypeName = "text")]
        public string? GenreName { get; set; }
    }

}
