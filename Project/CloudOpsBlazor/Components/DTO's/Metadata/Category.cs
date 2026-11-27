using System.Text.Json.Serialization;

namespace CloudOpsBlazor.Components
{
    public class Category
    {
        [JsonPropertyName("categorie_id")]
        public string CategoryId { get; set; } = "";

        [JsonPropertyName("categorie_name")]
        public string CategoryName { get; set; } = "";
    }
}
