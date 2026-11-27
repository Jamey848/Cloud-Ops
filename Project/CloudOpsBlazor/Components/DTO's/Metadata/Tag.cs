using System.Text.Json.Serialization;

namespace CloudOpsBlazor.Components
{
    public class Tag
    {
        [JsonPropertyName("tag_id")]
        public int TagId { get; set; }

        [JsonPropertyName("tag_name")]
        public string TagName { get; set; } = "";
    }
}
