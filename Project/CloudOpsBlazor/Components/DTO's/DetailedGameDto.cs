namespace CloudOpsBlazor.Components
{
    public class DetailedGameDto
    {
        // Basic information
        public long AppId { get; set; }
        public string? Name { get; set; }
        public string? ReleaseDate { get; set; }
        public long? RequiredAge { get; set; }
        public long? DlcCount { get; set; }
        public string? ShortDescription { get; set; }
        public string? HeaderImage { get; set; }

        // Platform support
        public bool? Windows { get; set; }
        public bool? Mac { get; set; }
        public bool? Linux { get; set; }

        // Additional information
        public long? Achievements { get; set; }
        public long? Recommendations { get; set; }
        public string? Notes { get; set; }
        public string? Developers { get; set; }
        public string? Publishers { get; set; }
        public double? CompositeScore { get; set; }

        // Related information
        public List<string> Genres { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public List<string> Categories { get; set; } = [];
        public List<string> Screenshots { get; set; } = [];
        public Game_popularity? PopDTO { get; set; }
        public Game_price? PriceDTO { get; set; }
        /*public Popularity Popularity { get; set; }
        public Price Prices { get; set; }*/ // Let this be known as a 'BFM', or in other words: 'A BIG FREAKING MISTAKE'
    }
}
