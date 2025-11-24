using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using statistics_reports.Models;

namespace statistics_reports.Services
{
    // Клиент для общения с Supabase из Blazor WASM
    public class SupabaseClient
    {
        // 👉 ВСТАВЬ СВОИ ЗНАЧЕНИЯ из Supabase (Settings → API)
        private const string SupabaseUrl = "https://psijvxsofuomogdunqvi.supabase.co";
        private const string SupabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InBzaWp2eHNvZnVvbW9nZHVucXZpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjM3MTU1NDAsImV4cCI6MjA3OTI5MTU0MH0.amPXylu3x0E-l4X1vu6N_C0IHJdQmhozNsILWLBgzng";

        private readonly SupabaseSession _session;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public SupabaseClient(SupabaseSession session)
        {
            _session = session;

            // HttpClient будет ходить только к Supabase
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(SupabaseUrl)
            };

            // Добавляем apikey ко всем запросам
            _httpClient.DefaultRequestHeaders.Add("apikey", SupabaseAnonKey);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // Применяем заголовок Authorization с access_token
        private void ApplyAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (_session.IsAuthenticated && !string.IsNullOrEmpty(_session.AccessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _session.AccessToken);
            }
        }

        // Логин в Supabase (email + пароль из Auth → Users)
        public async Task LoginAsync(string email, string password)
        {
            var url = "auth/v1/token?grant_type=password";

            var body = new
            {
                email,
                password
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(body)
            };

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка логина: {response.StatusCode} - {error}");
            }

            var authResponse = await response.Content.ReadFromJsonAsync<SupabaseAuthResponse>(_jsonOptions);
            if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.AccessToken))
            {
                throw new Exception("Supabase не вернул access_token.");
            }

            // 👉 сохраняем токен и в памяти, и в localStorage
            await _session.SetTokenAsync(authResponse.AccessToken);
        }


        // Получить всех работников из workers_table
        public async Task<List<Worker>> GetWorkersAsync()
        {
            if (!_session.IsAuthenticated)
                throw new Exception("Пользователь не авторизован.");

            ApplyAuthHeader();

            var url = "rest/v1/workers_table?select=*";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка получения работников: {response.StatusCode} - {error}");
            }

            var workers = await response.Content.ReadFromJsonAsync<List<Worker>>(_jsonOptions);
            return workers ?? new List<Worker>();
        }

        // Добавить нового работника
        public async Task<Worker?> InsertWorkerAsync(string name, string nr, string arbeitsplatz)
        {
            if (!_session.IsAuthenticated)
                throw new Exception("Пользователь не авторизован.");

            ApplyAuthHeader();

            var url = "rest/v1/workers_table";

            var body = new
            {
                name,
                nr,
                arbeitsplatz
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(body)
            };

            // Просим Supabase вернуть созданную запись
            request.Headers.Add("Prefer", "return=representation");

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка добавления работника: {response.StatusCode} - {error}");
            }

            var createdList = await response.Content.ReadFromJsonAsync<List<Worker>>(_jsonOptions);
            return createdList?.FirstOrDefault();
        }
    }
}

