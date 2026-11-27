using System.Text.Json.Serialization;

namespace CloudOpsBlazor.Components
{
    public class Genre
    {
        [JsonPropertyName("genre_id")]
        public string GenreId { get; set; } = "";

        [JsonPropertyName("genre_name")]
        public string GenreName { get; set; } = "";
    }
}
