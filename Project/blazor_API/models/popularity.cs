using Recommendations_engine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("popularity")]
    public class Popularity
    {
        [Key]
        [Column("pop_id", TypeName = "text")]
        public string PopId { get; set; } = null!;

        [Column("appid", TypeName = "bigint")]
        public long? AppId { get; set; }

        [Column("url", TypeName = "text")]
        public string? Url { get; set; }

        [Column("players.current", TypeName = "bigint")]
        public long? PlayersCurrent { get; set; }

        [Column("players.updatedAt", TypeName = "date")]
        public DateOnly? PlayersUpdatedAt { get; set; }

        [Column("players.allTimePeak", TypeName = "bigint")]
        public long? PlayersAllTimePeak { get; set; }

        [Column("players.allTimePeakAt", TypeName = "date")]
        public DateOnly? PlayersAllTimePeakAt { get; set; }

        [Column("reviews.total", TypeName = "bigint")]
        public long? ReviewsTotal { get; set; }

        [Column("reviews.positivePercent", TypeName = "double precision")]
        public double? ReviewsPositivePercent { get; set; }

        [Column("reviews.summary", TypeName = "text")]
        public string? ReviewsSummary { get; set; }

        public game? game { get; set; }
    }
}
