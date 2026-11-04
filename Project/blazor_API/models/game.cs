using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendations_engine
{
    [Table("games")]
    public class game
    {
        [Column("appid")]
        public long Appid { get; set; }
        [Column("name")]
        public string? Name { get; set; }

        [Column("release_date")]
        public string? Release_date { get; set; }

        [Column("required_age")]
        public long? Required_age { get; set; }

        [Column("dlc_count")]
        public long? Dlc_count { get; set; }

        [Column("short_description")]
        public string? Description { get; set; }

        [Column("windows")]
        public bool? Windows { get; set; }

        [Column("mac")]
        public bool? Mac { get; set; }

        [Column("linux")]
        public bool? Linux { get; set; }

        [Column("achievements")]
        public long? Achievements { get; set; }

        [Column("recommendations")]
        public long? Recommendations { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("developers")]
        public string? Developers { get; set; }

        [Column("publishers")]
        public string? Publishers { get; set; }

        [Column("composite_score")]
        public double? Score { get; set; }

        [Column("header_image")]
        public string? Header_image { get; set; }

    }
}
