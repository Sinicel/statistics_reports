window.supabaseAuth = {
    // Сохранить токен в localStorage
    setToken: function (token) {
        if (token) {
            localStorage.setItem('supabase_access_token', token);
        }
    },

    // Прочитать токен из localStorage
    getToken: function () {
        return localStorage.getItem('supabase_access_token');
    },

    // Удалить токен (выход)
    clearToken: function () {
        localStorage.removeItem('supabase_access_token');
    }
};
