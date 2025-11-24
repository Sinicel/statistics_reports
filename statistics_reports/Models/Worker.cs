using System.Text.Json.Serialization;

namespace statistics_reports.Models
{
    // Модель записи из таблицы workers_table
    public class Worker
    {
        [JsonPropertyName("id_w")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("nr")]
        public string Nr { get; set; } = string.Empty;

        [JsonPropertyName("arbeitsplatz")]
        public string Arbeitsplatz { get; set; } = string.Empty;
    }
}

