using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    public class Game_price
    {
        public double? InitialPrice { get; set; }

        public double? FinalPrice { get; set; }

        public long? Discount { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
