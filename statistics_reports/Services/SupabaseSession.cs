using Microsoft.JSInterop;

namespace statistics_reports.Services
{
    // Сервис сессии Supabase: хранит токен в памяти и в localStorage
    public class SupabaseSession
    {
        private readonly IJSRuntime _jsRuntime;

        public SupabaseSession(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Текущий токен в памяти
        public string? AccessToken { get; private set; }

        // Уже пробовали инициализироваться?
        public bool IsInitialized { get; private set; }

        // Признак того, что пользователь "залогинен"
        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);

        // Инициализация при старте (или при первом запросе) — читаем токен из localStorage
        public async Task InitializeAsync()
        {
            if (IsInitialized)
                return; // уже инициализировались, ничего не делаем

            try
            {
                AccessToken = await _jsRuntime.InvokeAsync<string?>("supabaseAuth.getToken");
            }
            catch
            {
                AccessToken = null;
            }

            IsInitialized = true;
        }

        // Установка токена после логина + сохранение в localStorage
        public async Task SetTokenAsync(string token)
        {
            AccessToken = token;

            try
            {
                await _jsRuntime.InvokeVoidAsync("supabaseAuth.setToken", token);
            }
            catch
            {
                // если не сохранилось в localStorage — не критично
            }

            IsInitialized = true;
        }

        // Очистка токена (выход)
        public async Task ClearAsync()
        {
            AccessToken = null;

            try
            {
                await _jsRuntime.InvokeVoidAsync("supabaseAuth.clearToken");
            }
            catch
            {
            }

            IsInitialized = true;
        }
    }
}
