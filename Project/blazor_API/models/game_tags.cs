using Recommendations_engine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("game_tags")]
    public class GameTag
    {
        [Key]
        [Column("gt_id", TypeName = "text")]
        public string GtId { get; set; } = null!;

        [Column("appid", TypeName = "bigint")]
        public long? AppId { get; set; }

        [Column("tag_id", TypeName = "bigint")]
        public long? TagId { get; set; }

        [Column("score", TypeName = "bigint")]
        public long? Score { get; set; }

        public Tag Linked_tag { get; set; }
        public game? game { get; set; }
    }
}
