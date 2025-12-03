// Утилита для печати только таблицы со всеми стилями/цветами
window.monthTable = {
    // Печать таблицы по id
    printTable: function (tableId) {
        const table = document.getElementById(tableId);
        if (!table) {
            // Если не нашли таблицу — печатаем страницу как есть
            window.print();
            return;
        }

        const win = window.open('', '_blank');

        // Подтягиваем все стили (<link rel="stylesheet">) из текущей страницы
        let stylesHtml = '';
        document.querySelectorAll('link[rel="stylesheet"]').forEach(function (link) {
            stylesHtml += link.outerHTML;
        });

        // Добавляем принт-правила, чтобы цвета сохранились при печати
        const printStyles = `
            <style>
                @media print {
                    body { margin: 0; -webkit-print-color-adjust: exact; print-color-adjust: exact; }
                    table { width: 100%; border-collapse: collapse; }
                    th, td {
                        border: 1px solid #a7a8aaff;
                        -webkit-print-color-adjust: exact;
                        print-color-adjust: exact;
                        text-align: center;
                    }
                    tr { border-bottom: 1px solid #a7a8aaff;}
                }
            </style>
        `;

        win.document.write('<html><head><title>Печать таблицы</title>');
        win.document.write(stylesHtml);
        win.document.write(printStyles);
        win.document.write('</head><body>');

        // Пишем только таблицу (без остальной страницы/JS)
        win.document.write(table.outerHTML);

        win.document.write('</body></html>');
        win.document.close();
        win.focus();

        win.print();
        win.close();
    }
};
