using Recommendations_engine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("screenshots")]
    public class Screenshot
    {
        [Key]
        [Column("screen_id", TypeName = "text")]
        public string ScreenId { get; set; } = null!;

        [Column("appid", TypeName = "bigint")]
        public long? AppId { get; set; }

        [Column("screenshot", TypeName = "text")]
        public string? ScreenshotUrl { get; set; }
        public game? game { get; set; }
    }
}
