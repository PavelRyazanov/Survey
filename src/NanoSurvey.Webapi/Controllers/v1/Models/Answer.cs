

using System.Text.Json.Serialization;

namespace NanoSurvey.Webapi.Models
{
    public class Answer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}