using Recommendations_engine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("game_categories")]
    public class GameCategory
    {
        [Key]
        [Column("gc_id", TypeName = "text")]
        public string GcId { get; set; } = null!;

        [Column("appid", TypeName = "bigint")]
        public long? AppId { get; set; }

        [Column("categorie_id", TypeName = "text")]
        public string? CategorieId { get; set; }

        public Category Linked_category { get; set; }
        public game? game { get; set; }
    }
}
