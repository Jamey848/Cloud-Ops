using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("tags")]
    public class Tag
    {
        [Key]
        [Column("tag_id", TypeName = "bigint")]
        public long TagId { get; set; }

        [Column("tag_name", TypeName = "text")]
        public string? TagName { get; set; }
    }
}
