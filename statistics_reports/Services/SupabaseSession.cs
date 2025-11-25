using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace statistics_reports.Services
{
    // Сервис сессии Supabase: хранит токен в памяти и в localStorage
    public class SupabaseSession
    {
        private readonly IJSRuntime _jsRuntime;

        // Событие, на которое могут подписываться компоненты (меню и т.п.),
        // чтобы реагировать на изменение состояния сессии.
        public event Action? OnChange;

        public SupabaseSession(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Внутреннее поле для токена
        private string? _accessToken;

        // Текущий токен в памяти.
        // При каждом изменении токена уведомляем подписчиков.
        public string? AccessToken
        {
            get => _accessToken;
            set
            {
                // Если значение не изменилось — выходим
                if (_accessToken == value)
                    return;

                _accessToken = value;

                // Уведомляем всех, кто подписался на изменения сессии
                NotifyStateChanged();
            }
        }

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
                var token = await _jsRuntime.InvokeAsync<string?>("supabaseAuth.getToken");
                AccessToken = token; // через свойство, чтобы сработал OnChange
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
            // Сохраняем в память (через свойство, чтобы триггернуть OnChange)
            AccessToken = token;

            try
            {
                await _jsRuntime.InvokeVoidAsync("supabaseAuth.setToken", token);
            }
            catch
            {
                // если не сохранилось в localStorage — не критично,
                // токен всё равно есть в памяти
            }

            IsInitialized = true;
        }

        // Очистка токена (выход)
        public async Task ClearAsync()
        {
            // Через свойство, чтобы сработал OnChange
            AccessToken = null;

            try
            {
                await _jsRuntime.InvokeVoidAsync("supabaseAuth.clearToken");
            }
            catch
            {
                // игнорируем ошибки JS — главное, что в памяти токен очищен
            }

            IsInitialized = true;
        }

        // Вызываем этот метод, когда изменяется состояние сессии
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
