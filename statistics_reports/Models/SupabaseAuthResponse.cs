using System.Text.Json.Serialization;

namespace statistics_reports.Models
{
    // Модель ответа Supabase Auth (нас интересует access_token)
    public class SupabaseAuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
