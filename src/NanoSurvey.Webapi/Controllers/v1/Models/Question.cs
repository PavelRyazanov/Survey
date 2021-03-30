

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NanoSurvey.Webapi.Models
{
    public class Question
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("answers")]
        public ICollection<Answer> Answers { get; set; }
    }
}