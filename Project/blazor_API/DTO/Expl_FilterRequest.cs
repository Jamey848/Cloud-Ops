namespace Recommendation_engine
{
    public class Expl_FilterRequest
    {
        public string? Searchname { get; set; }
        public long? AppID { get; set; }

        public List<string>? GenreIds { get; set; } = [];
        public List<string>? CategoryIds { get; set; } = [];
        public List<long>? TagIds { get; set; } = [];

        public double? MinPrice { get; set; }
    }
}