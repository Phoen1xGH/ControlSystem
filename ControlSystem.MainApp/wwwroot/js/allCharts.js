toastr.options = {
    "positionClass": "toast-bottom-right", // Устанавливаем позицию в нижний правый угол
};

function openChartInModeler(chartId) {
    var form = document.createElement('form');
    form.action = "/BPMNCharts/ModelerByChart";
    form.method = 'GET';

    var input = document.createElement('input');
    input.type = 'hidden';
    input.name = 'chartId';
    input.value = chartId;

    form.appendChild(input);
    document.body.appendChild(form);

    form.submit();
}

function delChart(chartId) {

    if (window.event) {
        window.event.stopPropagation(); // Для IE
    }
    else if (event) {
        event.stopPropagation(); // Современные браузеры
    }

    $.ajax({
        url: "/BPMNCharts/DeleteChart",
        method: "POST",
        data: { chartId: chartId },
        success: function () {

            $('#chart-div-' + chartId).remove();

            toastr.success('Изменения сохранены!');
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}