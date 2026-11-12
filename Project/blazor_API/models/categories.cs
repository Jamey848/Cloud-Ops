using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("categorie_id", TypeName = "text")]
        public string CategorieId { get; set; } = null!;

        [Column("categorie_name", TypeName = "text")]
        public string? CategorieName { get; set; }
    }
}
