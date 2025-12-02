// Глобальный объект для работы с печатью таблицы месяца
window.monthTable = {
    // Печать таблицы по её id
    printTable: function (tableId) {
        // Ищем таблицу на текущей странице
        var table = document.getElementById(tableId);
        if (!table) {
            // Если таблица не найдена — на всякий случай печатаем текущую страницу
            window.print();
            return;
        }

        // Открываем новое окно
        var win = window.open('', '_blank');

        // Собираем ВСЕ <link rel="stylesheet"> из текущего документа
        // (bootstrap, app.css, Statistics-Reports.styles.css и т.д.)
        var stylesHtml = '';
        document.querySelectorAll('link[rel="stylesheet"]').forEach(function (link) {
            stylesHtml += link.outerHTML;
        });

        // Формируем HTML печатного окна
        win.document.write('<html><head><title>Печать таблицы</title>');
        win.document.write(stylesHtml); // подключаем все те же стили, что и в основной странице
        win.document.write('</head><body>');

        // Вставляем только таблицу (со всеми классами и b-атрибутами Blazor)
        win.document.write(table.outerHTML);

        win.document.write('</body></html>');
        win.document.close();
        win.focus();

        // Диалог печати
        win.print();

        // Закрываем окно после печати (если не нужно — можно закомментировать)
        win.close();
    }
};
