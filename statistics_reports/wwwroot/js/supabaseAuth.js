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
    },

    //supabaseAuth = {
    //    setToken: function (token) {
    //        console.log('setToken called', token ? 'HAS TOKEN' : 'NO TOKEN');
    //        localStorage.setItem('supabaseToken', token);
    //    },
    //    getToken: function () {
    //        const t = localStorage.getItem('supabaseToken');
    //        console.log('getToken ->', t);
    //        return t;
    //    },
    //    clearToken: function () {
    //        console.log('clearToken');
    //        localStorage.removeItem('supabaseToken');
    //    }
    //}

}; 
