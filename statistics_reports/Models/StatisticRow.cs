using System;
using System.Text.Json.Serialization;


namespace statistics_reports.Models
{
    // Одна строка из таблицы statistics_table
    public class StatisticRow
    {
        [JsonPropertyName("id_st")]
        public long Id { get; set; }

        [JsonPropertyName("worker_id")]
        public long WorkerId { get; set; }

        [JsonPropertyName("work_date")]
        public DateTime WorkDate { get; set; }

        // Наши вычисляемые поля из базы
        [JsonPropertyName("weekday")]
        public int Weekday { get; set; }

        [JsonPropertyName("week_of_month")]
        public int WeekOfMonth { get; set; }

        [JsonPropertyName("menge")]
        public int Menge { get; set; }

        [JsonPropertyName("creat_date")]
        public DateTime CreatDate { get; set; }

        [JsonPropertyName("modific_date")]
        public DateTime ModificDate { get; set; }
    }
}
