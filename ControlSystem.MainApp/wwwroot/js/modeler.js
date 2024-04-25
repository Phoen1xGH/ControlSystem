import BpmnColorPickerModule from 'https://cdn.jsdelivr.net/npm/bpmn-js-color-picker@0.7.0/+esm';

var isEdit = document.getElementById('chartId').value != '' ? true : false;

if (isEdit) {
    $('#import-file-button').remove();
}

var xmlChart = `${document.getElementById("xmlChart").value}`;

// modeler instance
var bpmnModeler = new BpmnJS({
    container: '#canvas',
    keyboard: {
        bindTo: window
    },
    additionalModules: [
        BpmnColorPickerModule
    ]
});

openDiagram(xmlChart);

toastr.options = {
    "positionClass": "toast-bottom-right", // Устанавливаем позицию в нижний правый угол
};
/**  
 * Save diagram contents and print them to the console.
 */
async function exportDiagram() {

    try {

        var result = await bpmnModeler.saveXML({ format: true });

        alert('Diagram exported. Check the developer tools!');

        console.log('DIAGRAM', result.xml);
    } catch (err) {

        console.error('could not save BPMN 2.0 diagram', err);
    }
}

async function saveDiagramBpmn() {
    try {

        var result = await bpmnModeler.saveXML({ format: true });

        const xmlData = result.xml;
        const blob = new Blob([xmlData], { type: 'application/xml' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'diagram.bpmn';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);


    } catch (err) {

        console.error('could not save BPMN 2.0 diagram', err);
    }
}

function exportSvg() {
    bpmnModeler.saveSVG((err, svgData) => {
        if (err) {
            console.error('Не удалось экспортировать BPMN в SVG:', err);
        } else {
            const blob = new Blob([svgData], { type: 'image/svg+xml' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'diagram.svg';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);
        }
    });
}
/**
 * Open diagram in our modeler instance.
 *
 *  {String} bpmnXML diagram to display
 */
async function openDiagram(bpmnXML) {
    // import diagram
    try {

        await bpmnModeler.importXML(bpmnXML);

        // access modeler components
        var canvas = bpmnModeler.get('canvas');

        // zoom to fit full viewport
        canvas.zoom('fit-viewport');
    } catch (err) {

        console.error('could not import BPMN 2.0 diagram', err);
    }
}

async function exportChartToTickets() {

    var workspaceId = parseInt(document.getElementById('wList').value);
    var boardId = parseInt(document.getElementById('bList').value);
    var xmlData = await bpmnModeler.saveXML({ format: true });

    $.ajax({
        url: '/BPMNCharts/CreateTicketsFromChartTasks',
        method: 'POST',
        data: {
            workspaceId: workspaceId,
            boardId: boardId,
            xmlChart: xmlData.xml
        },
        success: function () {
            closeModalExport();
            toastr.success('Успешно экспортировано в карточки');
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function openModalExportToTickets() {
    var workspaces = JSON.parse(document.getElementById('workspaces').value);
    var workspacesSelectList = document.getElementById('wList');

    fillSelectList(workspacesSelectList, workspaces);

    var boards = JSON.parse(document.getElementById('boards').value);
    var boardsSelectList = document.getElementById('bList');

    var filteredBoards = boards.filter(function (board) {
        return board.WorkspaceId == workspaces[0].Id;
    });

    fillSelectList(boardsSelectList, filteredBoards);

    document.getElementById('modalExportToTickets').style.display = 'block';
    document.getElementById('overlayExport').style.display = 'block';
}

function fillSelectList(selectList, options) {
    options.forEach(function (option) {
        var optionElement = document.createElement('option');
        optionElement.value = option.Id;
        optionElement.textContent = option.Name;
        selectList.appendChild(optionElement);
    });

    if (selectList.options.length > 0) {
        selectList.selectedIndex = 0;
    }
}

document.getElementById('wList').addEventListener('change', function () {
    var workspaceId = parseInt(this.value);

    $('#bList').empty();
    var boards = JSON.parse(document.getElementById('boards').value);
    var filteredBoards = boards.filter(function (board) {
        return board.WorkspaceId == workspaceId;
    });

    // Заполняем список досок
    fillSelectList(document.getElementById('bList'), filteredBoards);
});

// wire save button
$('#save-button').click(exportDiagram);
$('#saveBpmn').click(saveDiagramBpmn);
$('#export-svg-button').click(exportSvg);
$('#import-file-button').click(uploadFile);
$('#save-to-db').click(doChartOperation);
$('#overlay').click(closeModal);
$('#overlayExport').click(closeModalExport);
$('#export-to-tickets').click(openModalExportToTickets);
$('#modalButtonExport').click(exportChartToTickets);

function doChartOperation() {
    var isEdit = document.getElementById('chartId').value != '' ? true : false;
    if (!isEdit) {
        openModal();
    }
    else {
        editChart(document.getElementById('chartId').value);
    }
}

async function editChart(chartId) {

    var xml = await bpmnModeler.saveXML({ format: true });

    $.ajax({
        url: '/BPMNCharts/EditChart',
        method: 'POST',
        data: { chartId: chartId, newXmlData: xml.xml },
        success: function () {
            toastr.success('Изменения сохранены!');
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function openModal() {
    document.getElementById('myModal').style.display = 'block';
    document.getElementById('overlay').style.display = 'block';
}

function closeModal() {
    document.getElementById('myModal').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
}

function closeModalExport() {
    document.getElementById('modalExportToTickets').style.display = 'none';
    document.getElementById('overlayExport').style.display = 'none';

    $('#bList').empty();
    $('#wList').empty();
}

document.getElementById("modalButton").addEventListener("click", async function () {
    var xmlData = await bpmnModeler.saveXML({ format: true });

    var title = document.getElementById('modalInput').value;

    // var xmlString = new XMLSerializer().serializeToString(xmlData);

    let chart = {
        Title: title,
        XmlData: xmlData.xml,
    };
    var jsonData = JSON.stringify(chart);

    document.getElementById("jsonChart").value = jsonData;
});


function uploadFile() {
    var fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = '.bpmn, .xml'
    fileInput.style.display = 'none';
    fileInput.multiple = false;

    document.body.appendChild(fileInput);

    fileInput.addEventListener('change', function () {
        // Получаем выбранные файлы
        var file = fileInput.files[0];

        // Создаем объект FormData и добавляем все файлы
        var formData = new FormData();

        formData.append('file', file);

        // Создаем объект XMLHttpRequest
        var xhr = new XMLHttpRequest();

        // Настраиваем запрос
        xhr.open('POST', '/BPMNCharts/ImportBPMNFile', true);

        // Обработка завершения запроса
        xhr.onload = function () {
            if (xhr.status === 200) {
                var xmlFile = JSON.parse(xhr.responseText);

                openDiagram(xmlFile);

                toastr.success('Файл импортирован!');
            } else {
                toastr.error(xhr.responseText || 'Произошла ошибка!');
            }

            // Удаляем временный input
            document.body.removeChild(fileInput);
        };

        // Отправляем запрос на сервер с данными FormData
        xhr.send(formData);
    });

    // Запускаем окно выбора файлов
    fileInput.click();
}