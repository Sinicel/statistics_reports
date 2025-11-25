// Простой модуль для работы с графиком статистики
window.statisticsChart = (function () {

    // Храним текущий экземпляр графика, чтобы корректно перерисовывать
    let chartInstance = null;

    /**
     * Рисуем или перерисовываем график
     * @param {string} canvasId - id элемента <canvas>
     * @param {Array<string>} labels - подписи по оси X (например, дни месяца)
     * @param {Array<number>} data - значения по оси Y (количество)
     */
    function render(canvasId, labels, data) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.warn("Не найден canvas с id:", canvasId);
            return;
        }

        const ctx = canvas.getContext('2d');
        if (!ctx) {
            console.warn("Не удалось получить 2D-контекст для canvas:", canvasId);
            return;
        }

        // Если график уже есть — уничтожаем, чтобы не рисовать поверх
        if (chartInstance) {
            chartInstance.destroy();
            chartInstance = null;
        }

        chartInstance = new Chart(ctx, {
            type: 'bar', // при желании можно заменить на 'line'
            data: {
                labels: labels,
                datasets: [{
                    label: 'Количество за день',
                    data: data
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'День месяца'
                        }
                    },
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Количество'
                        }
                    }
                }
            }
        });
    }

    return {
        render: render
    };
})();
