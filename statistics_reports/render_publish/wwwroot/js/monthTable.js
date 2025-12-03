// Утилита для печати только таблицы со всеми стилями/цветами
window.monthTable = {
    printTable: function (tableId) {
        const table = document.getElementById(tableId);
        if (!table) {
            window.print();
            return;
        }

        const win = window.open('', '_blank');

        // Берём все стили из <head> (и link, и style)
        const stylesHtml = Array.from(
            document.head.querySelectorAll('link[rel="stylesheet"], style')
        ).map(el => el.outerHTML).join('');

        // Добавляем принт-правила, чтобы цвета и бордеры дошли до принтера
        const printStyles = `
  <style>
    @media print {
      body { margin: 0; -webkit-print-color-adjust: exact; print-color-adjust: exact; }
      table { width: 100%; border-collapse: collapse; }
      th, td {
        border: 1px solid #a7a8aa;
        -webkit-print-color-adjust: exact;
        print-color-adjust: exact;
        text-align: center;
      }
      tr { border-bottom: 1px solid #a7a8aa; }

      /* восстанавливаем bootstrap-цвет в принте */
      .bg-warning {
        background-color: #ffc107 !important;
        color: #212529; /* контрастный текст */
        -webkit-print-color-adjust: exact;
        print-color-adjust: exact;
      }
    }
  </style>
`;


        win.document.write('<html><head><title>Печать таблицы</title>');
        win.document.write(stylesHtml);
        win.document.write(printStyles);
        win.document.write('</head><body>');
        win.document.write(table.outerHTML); // классы и inline-стили уже внутри
        win.document.write('</body></html>');
        win.document.close();
        win.focus();
        win.print();
        win.close();
    }
};
