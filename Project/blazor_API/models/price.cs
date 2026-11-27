using Recommendations_engine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("price")]
    public class Price
    {
        [Key]
        [Column("price_id", TypeName = "text")]
        public string PriceId { get; set; } = null!;

        [Column("appid", TypeName = "bigint")]
        public long? AppId { get; set; }

        [Column("initial_price", TypeName = "double precision")]
        public double? InitialPrice { get; set; }

        [Column("final_price", TypeName = "double precision")]
        public double? FinalPrice { get; set; }

        [Column("discount", TypeName = "bigint")]
        public long? Discount { get; set; }

        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTimeOffset? UpdatedAt { get; set; }
        public game? game { get; set; }
    }
}
