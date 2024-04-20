function openModal() {
    //ПОСЛЕ ДОБАВЛЕНИЯ AJAX
    //РАСКОММЕНТИТЬ ЭТО:

    //var modal = document.getElementById('myModal');
    //var form = modal.querySelector('form');
    //form.action = '/Workspace/CreateWorkspace';

    document.getElementById('myModal').style.display = 'block';
    document.getElementById('overlay').style.display = 'block';
}

function closeModal() {
    document.getElementById('myModal').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
}

function editWorkspace(id) {
    var modal = document.getElementById('myModal');

    // Находим форму внутри модального окна
    var form = modal.querySelector('form');
    form.action = '/Workspace/RenameWorkspace';
    form.querySelector('input[name="id"]').value = id;

    openModal();
}

//----------------------------------------------------------------------------------
function openModalBoard() {
    //ПОСЛЕ ДОБАВЛЕНИЯ AJAX
    //РАСКОММЕНТИТЬ ЭТО:

    //var modal = document.getElementById('myModal');
    //var form = modal.querySelector('form');
    //form.action = '/Workspace/CreateWorkspace';

    document.getElementById('myModalBoard').style.display = 'block';
    document.getElementById('overlayBoard').style.display = 'block';
}

function closeModalBoard() {
    document.getElementById('myModalBoard').style.display = 'none';
    document.getElementById('overlayBoard').style.display = 'none';
}

function setCurrentWorkspace(id) {
    document.querySelector('input[name="currentWorkspace"]').value = id;
}

function getWorkspaceId() {
    var modal = document.getElementById('myModalBoard');

    // Находим форму внутри модального окна
    var form = modal.querySelector('form');
    form.querySelector('input[name="workspaceId"]').value = document.querySelector('input[name="currentWorkspace"]').value;
}

function editBoard(id, color) {
    var modal = document.getElementById('myModalBoard');
    var form = modal.querySelector('form');
    form.action = '/Workspace/EditBoard';
    form.querySelector('input[name="boardId"]').value = id;
    form.querySelector('input[name="ColorHex"]').value = color;

    openModalBoard();
}

function openModalTicket(id) {
    var modal = document.getElementById('myModalTicket');
    var form = modal.querySelector('form');
    form.querySelector('input[name="StatusId"]').value = id;
    document.getElementById('myModalTicket').style.display = 'block';
    document.getElementById('overlayTicket').style.display = 'block';
}

function closeModalTicket() {
    document.getElementById('myModalTicket').style.display = 'none';
    document.getElementById('overlayTicket').style.display = 'none';
    var currentUrl = window.location.href;

    // Регулярное выражение для удаления последнего сегмента URL
    var regex = /\/\d+$/;

    // Обрезаем URL до необходимого уровня
    var trimmedUrl = currentUrl.replace(regex, '');

    // Заменяем текущее состояние истории новым состоянием с обновленным URL
    history.replaceState(null, null, trimmedUrl);

    var modal = document.getElementById('myModalTicket');
    var form = modal.querySelector('form');
    var id = form.querySelector('input[name="StatusId"]').value;

    editTicket(id);
}



function initializeTabs() {

    var tabs = document.querySelectorAll('.tab');
    var tabContents = document.querySelectorAll('.tab-content');

    tabs.forEach(function (tab) {
        tab.addEventListener('click', function () {
            var tabId = this.getAttribute('data-tab');

            tabs.forEach(function (tab) {
                tab.classList.remove('active');
            });

            tabContents.forEach(function (content) {
                content.classList.remove('active');
            });

            this.classList.add('active');
            document.getElementById(tabId).classList.add('active');
        });
    });
}


//function auroResize() {
//    const textarea = document.getElementById("inner-textarea");
//    textarea.style.height = 'auto';
//    textarea.style.height = (textarea.scrollHeight <= 150) ? textarea.scrollHeight + 'px' : '150px';
//}

function auroResize() {
    const textarea = document.getElementById("inner-textarea");

    // Получаем количество строк в textarea
    const numberOfLines = (textarea.value.match(/\n/g) || []).length + 1;

    // Устанавливаем высоту только если количество строк больше 1
    textarea.style.height = (numberOfLines > 1) ? 'auto' : '50px';

    // Если количество строк больше 1, устанавливаем высоту в соответствии с содержимым
    if (numberOfLines > 1) {
        const scrollHeight = textarea.scrollHeight;
        textarea.style.height = (scrollHeight <= 150) ? scrollHeight + 'px' : '150px';

        // Принудительно обновляем значение textarea
        textarea.value = textarea.value;
    }
}


function setupDescriptionInput() {
    var descriptionDiv = document.querySelector('.description-text-container');
    var descriptionHidden = document.getElementById('hiddenDescription');

    descriptionDiv.addEventListener('input', function () {
        descriptionHidden.value = descriptionDiv.textContent;
    });
}

//document.addEventListener('DOMContentLoaded', function () {
//    setupDescriptionInput();
//});


// Ajax-------------------------------------------------------------------------------

toastr.options = {
    "positionClass": "toast-bottom-right", // Устанавливаем позицию в нижний правый угол
};

function createTicket() {
    var formData = $("#formTicket").serialize();

    $.ajax({
        type: 'POST',
        url: '/Ticket/CreateTicket',
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        dataType: 'json',
        data: formData,
        success: function (result) {

            var ticketsDiv = $('#board-' + result[0].statusId);
            ticketsDiv.empty();
            result.forEach(function (ticket) {
                ticketsDiv.append('<div class="ticket"><label style="color: #d8d8d8">' + ticket.title + '</label></div>');
            });

            toastr.success('Тикет успешно создан!');
        },
        error: function (xhr, status, error) {
            //console.error('HTTP error occurred:', status, error);
            //console.error('Response Text:', xhr.responseText);
            //console.error('Status Text:', xhr.statusText);
            //console.error('Full response:', xhr);
            toastr.error(JSON.parse(xhr.responseText).trim() || 'Произошла ошибка!');
            //toastr.error('Произошла ошибка при создании тикета.');
        }
    });
}

//-------------------------------------------------------------------------------

function addNewTicket(id) {
    var newTicketHtml = '<div class="ticket" contenteditable="true" onkeydown="handleKeyDown(event, ' + id + ')"></div>';
    var $board = $("#board-" + id);

    $board.prepend(newTicketHtml).promise().done(function () {
        $board.find(".ticket:first").focus();
    });
}


function handleKeyDown(event, id) {
    if (event.key === "Enter") {
        event.preventDefault();
        $(event.target).blur();
        var text = event.target.innerText.trim();


        if (text !== "") {
            createTicketV2(text, id);
            //console.log("Выполнить AJAX-запрос с текстом:", text);
        }
    }
}

function createTicketV2(text, boardId) {

    $.ajax({
        type: 'POST',
        url: '/Ticket/CreateTicket',
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        dataType: 'json',
        data: { title: text, boardId: boardId },
        success: function (result) {

            var ticketsDiv = $('#board-' + result[0].statusId);
            ticketsDiv.empty();

            result.forEach(function (ticket) {
                var ticketElement = `
                                <div class="ticket" id="ticket-${ticket.id}" onclick="openTicket(${ticket.workspaceId}, ${ticket.id})">
                                    <div class="ticket-title-preview">
                                        <label id="ticket-preview-label">${ticket.title}</label>
                                        <button id="ticket-del-button" onclick="deleteTicket(${ticket.statusId}, ${ticket.id})">
                                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"/>
                                            </svg>
                                        </button>
                                    </div>
                                `;
                if (ticket.priority != null) {

                    ticketElement += `
                                <div class="ticket-preview-priority">
                                    <svg class="prio-image" width="20px" height="20px" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg">
                                        <path fill="#d8d8d8" d="M288 128h608L736 384l160 256H288v320h-96V64h96v64z" />
                                    </svg>
                                    <div style="margin-left: 7px; padding-left: 7px; padding-right: 7px; background-color: ${ticket.priority.colorHex}; border-radius: 20px;">
                                        <label style="font-weight: bold; color: black">${ticket.priority.name}</label>
                                    </div>
                                </div>`;
                }

                ticketElement += `</div>`;
                // Добавляем элемент в ticketsDiv
                ticketsDiv.append(ticketElement);
            });

            toastr.success('Тикет успешно создан!');
        },
        error: function (xhr, status, error) {
            //console.error('HTTP error occurred:', status, error);
            //console.error('Response Text:', xhr.responseText);
            //console.error('Status Text:', xhr.statusText);
            //console.error('Full response:', xhr);
            toastr.error(JSON.parse(xhr.responseText).trim() || 'Произошла ошибка!');
            //toastr.error('Произошла ошибка при создании тикета.');
        }
    });
}


function openTicket(workspaceId, ticketId) {
    var currentPath = window.location.pathname;

    // Проверяем, содержится ли workspaceId в текущем пути
    if (currentPath.indexOf(workspaceId) === -1) {
        // Если не содержится, добавляем workspaceId к текущему пути
        currentPath += '/' + workspaceId;
    }

    var ticketUrl = currentPath + '/' + ticketId;

    history.pushState({}, '', ticketUrl);

    // Загружаем данные через AJAX
    loadTicketData(workspaceId, ticketId);
}


function loadTicketData(workspaceId, ticketId) {
    // Используем библиотеку jQuery для упрощения AJAX-запроса
    $.ajax({
        url: "/Ticket/TicketDetails",
        //url: workspaceId +'/' + ticketId,
        method: 'GET',
        data: { workspaceId: workspaceId, ticketId: ticketId },
        success: function (data) {
            openTicketModal(data);
        },
        error: function (xhr, status, error) {
            toastr.error(JSON.parse(xhr.responseText).trim() || 'Произошла ошибка!');
        }
    });
}

function openTicketModal(partialViewHtml) {
    // Ваш код для открытия модального окна
    var modal = document.getElementById('myModalTicket');
    var overlay = document.getElementById('overlayTicket');

    // Заполняем содержимым Partial View
    modal.innerHTML = partialViewHtml;

    modal.style.display = 'block';
    overlay.style.display = 'block';

    setupDescriptionInput();
    initializeTabs();
    refactorComments();
}

function refactorComments() {
    $(".comment-text").each(function () {
        var content = $(this).text();
        content = content.replace(/\n/g, "<br>");
        $(this).html(content);
    });
}


document.addEventListener("DOMContentLoaded", function () {
    var ticketId = document.getElementById("ticketId").value;
    var workspaceId = document.getElementById("workspaceId").value;

    if (ticketId != 0) {
        loadTicketData(workspaceId, ticketId);
    }
});

function editTicket(boardId) {
    var formData = $("#formTicket").serialize();

    $.ajax({
        url: "/Ticket/EditTicket",
        method: 'POST',
        data: formData,
        success: function (result) {
            var ticketsDiv = $('#board-' + result[0].statusId);
            ticketsDiv.empty();
            console.log(result);
            result.forEach(function (ticket) {
                var ticketElement = `
                                <div class="ticket" id="ticket-${ticket.id}" onclick="openTicket(${ticket.workspaceId}, ${ticket.id})">
                                    <div class="ticket-title-preview">
                                        <label id="ticket-preview-label">${ticket.title}</label>
                                        <button id="ticket-del-button" onclick="deleteTicket(${ticket.statusId}, ${ticket.id})">
                                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"/>
                                            </svg>
                                        </button>
                                    </div>
                                `;
                if (ticket.priority != null) {

                    ticketElement += `
                                <div class="ticket-preview-priority">
                                    <svg class="prio-image" width="20px" height="20px" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg">
                                        <path fill="#d8d8d8" d="M288 128h608L736 384l160 256H288v320h-96V64h96v64z" />
                                    </svg>
                                    <div style="margin-left: 7px; padding-left: 7px; padding-right: 7px; background-color: ${ticket.priority.colorHex}; border-radius: 20px;">
                                        <label style="font-weight: bold; color: black">${ticket.priority.name}</label>
                                    </div>
                                </div>`;
                }

                ticketElement += `</div>`;
                // Добавляем элемент в ticketsDiv
                ticketsDiv.append(ticketElement);
            });

            toastr.success('Изменения сохранены!');
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function deleteTicket(boardId, ticketId) {

    if (window.event) {
        window.event.stopPropagation(); // Для IE
    }
    else if (event) {
        event.stopPropagation(); // Современные браузеры
    }

    $.ajax({
        url: "/Ticket/DeleteTicket",
        method: "POST",
        data: { boardId: boardId, ticketId: ticketId },
        success: function (result) {

            $('#ticket-' + ticketId).remove();

            toastr.success('Задача успешно удалена!');
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}


function openModalPriority(partialViewHtml) {
    var modal = document.getElementById('modal-priority');
    var overlay = document.getElementById('overlayPriority');

    // Заполняем содержимым Partial View
    modal.innerHTML = partialViewHtml;

    modal.style.display = 'block';
    overlay.style.display = 'block';
}

function closeModalPriority() {
    var modal = document.getElementById('modal-priority');
    var overlay = document.getElementById('overlayPriority');

    modal.style.display = 'none';
    overlay.style.display = 'none';
}

function getPriorityData(ticketId) {

    $.ajax({
        url: "/Priority/GetPriorities",
        method: "POST",
        data: { ticketId: ticketId },
        success: function (result) {
            openModalPriority(result);
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function choosePrio(ticketId, priorityId) {
    $.ajax({
        url: "/Priority/AddPriorityToTicket",
        method: "POST",
        data: { ticketId: ticketId, priorityId: priorityId },
        success: function (result) {

            // Полностью обновляем контейнер с приоритетом
            var priorityContainer = $(".priority-container");

            priorityContainer.html(`
                    <input type="hidden" name="Priority.Name" value="${result.name}" />
                    <input type="hidden" name="Priority.ColorHex" value="${result.colorHex}" />
                    <input type="hidden" name="Priority.Id" value="${result.id}" />

                    <svg class="prio-image" width="20px" height="20px" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg">
                        <path fill="#d8d8d8" d="M288 128h608L736 384l160 256H288v320h-96V64h96v64z" />
                    </svg>
                    <label style="color: #d8d8d8; margin-left: 7px;">Приоритет: </label>
                    <div style="margin-left: 7px; padding-left: 7px; padding-right: 7px; background-color: ${result.colorHex}; border-radius: 20px;">
                        <label style="font-weight: bold">${result.name}</label>
                    </div>
                `);

        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}


function addNewPrio(ticketId) {
    var newPrioHtml = `
        <div id="prio-data-div">
            <input id="prio-name" type="text" class="input" name="Name" placeholder=""/>
            <label class="placeholder">Название</label>
            <label style="margin-left: 20px; color: #d8d8d8">Цвет</label>
            <input type="color" name="ColorHex" class="color-input"/>
            <button id="accept-new-prio" onclick="acceptNewPrio(${ticketId})">
                <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" fill="#15c118">
                    <path d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z" />
                </svg>
            </button>

            <button id="cancel-new-prio" onclick="cancelNewPrio()">
                <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 384 512" fill="#f01313">
                    <path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z" />
                </svg>
            </button>
        </div>`;

    var prioContainer = $("#prio-container");

    prioContainer.prepend(newPrioHtml).promise().done(function () {
        prioContainer.find("#prio-name").focus();
    });
}

function cancelNewPrio() {
    $("#prio-data-div").remove();
}

function acceptNewPrio(ticketId) {
    var priority = {
        ColorHex: $(".color-input").val(),
        Name: $("#prio-name").val()
    };

    $.ajax({
        url: "/Priority/CreatePriority",
        method: "POST",
        data: priority,
        success: function (result) {
            $("#prio-container").empty();

            result.forEach(function (prio) {
                var priorityDiv = `
                        <div id="prio-flex-wrapper">
                            <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${prio.colorHex}; border-radius: 20px; width: fit-content;"
                            onclick="choosePrio(${ticketId}, ${prio.id})">
                                <label style="font-weight: bold">${prio.name}</label>
                            </div>
                            <button id="del-prio-btn" onclick="delPrio(${ticketId}, ${prio.id})">
                                <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                        <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"/>
                                    </svg>
                            </button>
                        </div>`;

                $("#prio-container").append(priorityDiv);
            });
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function delPrio(ticketId, prioId) {

    $.ajax({
        url: "/Priority/DeletePriority",
        method: "POST",
        data: { priorityId: prioId },
        success: function (result) {
            $("#prio-container").empty();

            result.forEach(function (prio) {
                var priorityDiv = `
                            <div id="prio-flex-wrapper">
                                <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${prio.colorHex}; border-radius: 20px; width: fit-content;"
                                onclick="choosePrio(${ticketId}, ${prio.id})">
                                    <label style="font-weight: bold">${prio.name}</label>
                                </div>
                                <button id="del-prio-btn" onclick="delPrio(${ticketId}, ${prio.id})">
                                    <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                            <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"/>
                                        </svg>
                                </button>
                            </div>`;

                $("#prio-container").append(priorityDiv);
            });

        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}


function openModalTags(partialViewHtml) {
    var modal = document.getElementById('modal-tags');
    var overlay = document.getElementById('overlayTags');

    // Заполняем содержимым Partial View
    modal.innerHTML = partialViewHtml;

    modal.style.display = 'block';
    overlay.style.display = 'block';
}

function closeModalTags() {
    var modal = document.getElementById('modal-tags');
    var overlay = document.getElementById('overlayTags');

    modal.style.display = 'none';
    overlay.style.display = 'none';
}

function getTagsData(ticketId) {

    $.ajax({
        url: "/Tags/GetTags",
        method: "GET",
        data: { ticketId: ticketId },
        success: function (result) {

            openModalTags(result);
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function addNewTag(ticketId) {
    var newTagHtml = `
            <div id="tag-data-div">
                <input id="tag-name" type="text" class="input" name="Name" placeholder=""/>
                <label class="placeholder">Название</label>
                <label style="margin-left: 20px; color: #d8d8d8">Цвет</label>
                <input type="color" name="ColorHex" class="color-input"/>
                <button id="accept-new-tag" onclick="acceptNewTag(${ticketId})">
                    <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" fill="#15c118">
                        <path d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z" />
                    </svg>
                </button>

                <button id="cancel-new-tag" onclick="cancelNewTag()">
                    <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 384 512" fill="#f01313">
                        <path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z" />
                    </svg>
                </button>
            </div>`;

    $("#add-tag-wrapper").append(newTagHtml).promise().done(function () {
        $("#add-tag-wrapper").find("#tag-name").focus();
    });
}

function cancelNewTag() {
    $("#tag-data-div").remove();
}

function acceptNewTag(ticketId) {

    var tagData = {
        ColorHex: $(".color-input").val(),
        Name: $("#tag-name").val()
    };

    $.ajax({
        url: "/Tags/CreateTag",
        method: "POST",
        data: { ticketId: ticketId, tagData: tagData },
        success: function (result) {
            $("#tag-data-div").remove();

            $("#all-tags-container").empty();

            result.unusedTags.forEach(function (tag) {
                var tagDiv = `
                    <div id="tag-flex-wrapper">
                        <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                             onclick="chooseTag(${ticketId}, ${tag.id})">
                            <label style="font-weight: bold">${tag.name}</label>
                        </div>
                            <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                            </svg>
                        </button>
                    </div>`;

                $("#all-tags-container").append(tagDiv);
            });

            $("#used-tags-container").empty();

            result.usedTags.forEach(function (tag) {
                var tagDiv = `
                    <div id="tag-flex-wrapper">
                        <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                             onclick="removeTag(${ticketId}, ${tag.id})">
                            <label style="font-weight: bold">${tag.name}</label>
                        </div>
                        <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                            </svg>
                        </button>
                    </div>`;

                $("#used-tags-container").append(tagDiv);
            });

        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function chooseTag(ticketId, tagId) {

    $.ajax({
        url: "/Tags/AddTagToTicket",
        method: "POST",
        data: { ticketId: ticketId, tagId: tagId },
        success: function (result) {

            $(".tags").empty();
            $("#used-tags-container").empty();
            $("#all-tags-container").empty();

            $(".tags").append(`
                    <svg class="tags-image" fill="#d8d8d8" width="20px" height="20px" viewBox="0 0 256 256" xmlns="http://www.w3.org/2000/svg">
                        <path d="M246.65625,132.43713l-45.625,68.43848.001-.001a15.96649,15.96649,0,0,1-13.31348,7.125H40a16.01833,16.01833,0,0,1-16-16v-128a16.01833,16.01833,0,0,1,16-16H187.71875a15.9687,15.9687,0,0,1,13.31348,7.126l45.624,68.43652A7.99771,7.99771,0,0,1,246.65625,132.43713Z" />
                    </svg>
                    <label style="color: #d8d8d8; padding-left: 7px; padding-right: 7px">Теги:</label>`);

            var index = 0;

            result.usedTags.forEach(function (tag) {

                var tagDiv = `
                    <div class="tag" style="background-color: ${tag.colorHex}; border-radius: 20px 20px 20px 20px; padding-right: 15px; padding-left:15px; margin-left: 7px;">

                        <input type="hidden" name="Tags[${index}].ColorHex" value="${tag.colorHex}">
                        <input type="hidden" name="Tags[${index}].Name" value="${tag.name}">
                        <input type="hidden" name="Tags[${index}].Id" value="${tag.id}">

                        <label style="font-weight: bold">${tag.name}</label>
                    </div>`;

                $(".tags").append(tagDiv);


                var tagDiv = `
                    <div id="tag-flex-wrapper">
                        <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                             onclick="removeTag(${ticketId}, ${tag.id})">
                            <label style="font-weight: bold">${tag.name}</label>
                        </div>
                        <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                            </svg>
                        </button>
                    </div>`;

                $("#used-tags-container").append(tagDiv);

                ++index;
            });

            //$(".tags").append(`
            //     <button id="add-button" style="margin-left: 7px">
            //        <svg id="add-button-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
            //            <path d="M256 80c0-17.7-14.3-32-32-32s-32 14.3-32 32V224H48c-17.7 0-32 14.3-32 32s14.3 32 32 32H192V432c0 17.7 14.3 32 32 32s32-14.3 32-32V288H400c17.7 0 32-14.3 32-32s-14.3-32-32-32H256V80z" />
            //        </svg>
            //    </button>`);


            $("#all-tags-container").empty();

            result.unusedTags.forEach(function (tag) {
                var tagDiv = `
                    <div id="tag-flex-wrapper">
                        <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                             onclick="chooseTag(${ticketId}, ${tag.id})">
                            <label style="font-weight: bold">${tag.name}</label>
                        </div>
                            <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                            </svg>
                        </button>
                    </div>`;

                $("#all-tags-container").append(tagDiv);
            });

        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function removeTag(ticketId, tagId) {
    $.ajax({
        url: "/Tags/RemoveTagFromTicket",
        method: "POST",
        data: { ticketId: ticketId, tagId: tagId },
        success: function (result) {

            $(".tags").empty();
            $("#used-tags-container").empty();
            $("#all-tags-container").empty();

            $(".tags").append(`
                        <svg class="tags-image" fill="#d8d8d8" width="20px" height="20px" viewBox="0 0 256 256" xmlns="http://www.w3.org/2000/svg">
                            <path d="M246.65625,132.43713l-45.625,68.43848.001-.001a15.96649,15.96649,0,0,1-13.31348,7.125H40a16.01833,16.01833,0,0,1-16-16v-128a16.01833,16.01833,0,0,1,16-16H187.71875a15.9687,15.9687,0,0,1,13.31348,7.126l45.624,68.43652A7.99771,7.99771,0,0,1,246.65625,132.43713Z" />
                        </svg>
                        <label style="color: #d8d8d8; padding-left: 7px; padding-right: 7px">Теги:</label>`);

            var index = 0;

            result.usedTags.forEach(function (tag) {

                var tagDiv = `
                        <div class="tag" style="background-color: ${tag.colorHex}; border-radius: 20px 20px 20px 20px; padding-right: 15px; padding-left:15px; margin-left: 7px;">

                            <input type="hidden" name="Tags[${index}].ColorHex" value="${tag.colorHex}">
                            <input type="hidden" name="Tags[${index}].Name" value="${tag.name}">
                            <input type="hidden" name="Tags[${index}].Id" value="${tag.id}">

                            <label style="font-weight: bold">${tag.name}</label>
                        </div>`;

                $(".tags").append(tagDiv);

                var tagDiv = `
                    <div id="tag-flex-wrapper">
                        <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                         onclick="removeTag(${ticketId}, ${tag.id})">
                            <label style="font-weight: bold">${tag.name}</label>
                        </div>
                        <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                            </svg>
                        </button>
                    </div>`;

                $("#used-tags-container").append(tagDiv);

                ++index;
            });

            $(".tags").append(`
                         <button id="add-button" style="margin-left: 7px">
                            <svg id="add-button-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                                <path d="M256 80c0-17.7-14.3-32-32-32s-32 14.3-32 32V224H48c-17.7 0-32 14.3-32 32s14.3 32 32 32H192V432c0 17.7 14.3 32 32 32s32-14.3 32-32V288H400c17.7 0 32-14.3 32-32s-14.3-32-32-32H256V80z" />
                            </svg>
                        </button>`);


            result.unusedTags.forEach(function (tag) {
                var tagDiv = `
                    <div id="tag-flex-wrapper">
                        <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                         onclick="chooseTag(${ticketId}, ${tag.id})">
                            <label style="font-weight: bold">${tag.name}</label>
                        </div>
                            <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                            </svg>
                        </button>
                    </div>`;

                $("#all-tags-container").append(tagDiv);
            });

        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function delTag(ticketId, tagId) {

    $.ajax({
        url: "/Tags/DeleteTag",
        method: "POST",
        data: { ticketId: ticketId, tagId: tagId },
        success: function (result) {
            $("#tag-data-div").remove();

            $("#all-tags-container").empty();
            $("#used-tags-container").empty();

            result.unusedTags.forEach(function (tag) {
                var tagDiv = `
                        <div id="tag-flex-wrapper">
                            <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                             onclick="chooseTag(${ticketId}, ${tag.id})">
                                <label style="font-weight: bold">${tag.name}</label>
                            </div>
                            <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                                <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                                </svg>
                            </button>
                        </div>`;

                $("#all-tags-container").append(tagDiv);
            });



            result.usedTags.forEach(function (tag) {
                var tagDiv = `
                        <div id="tag-flex-wrapper">
                            <div style="margin-top: 7px; padding-left: 7px; padding-right: 7px; background-color: ${tag.colorHex}; border-radius: 20px; width: fit-content;"
                             onclick="removeTag(${ticketId}, ${tag.id})">
                                <label style="font-weight: bold">${tag.name}</label>
                            </div>
                                <button id="del-tag-btn" onclick="delTag(${ticketId}, ${tag.id})">
                                <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
                                </svg>
                            </button>
                        </div>`;

                $("#used-tags-container").append(tagDiv);
            });

            $(".tags").empty();

            $(".tags").append(`
                            <svg class="tags-image" fill="#d8d8d8" width="20px" height="20px" viewBox="0 0 256 256" xmlns="http://www.w3.org/2000/svg">
                                <path d="M246.65625,132.43713l-45.625,68.43848.001-.001a15.96649,15.96649,0,0,1-13.31348,7.125H40a16.01833,16.01833,0,0,1-16-16v-128a16.01833,16.01833,0,0,1,16-16H187.71875a15.9687,15.9687,0,0,1,13.31348,7.126l45.624,68.43652A7.99771,7.99771,0,0,1,246.65625,132.43713Z" />
                            </svg>
                            <label style="color: #d8d8d8; padding-left: 7px; padding-right: 7px">Теги:</label>`);

            var index = 0;

            result.usedTags.forEach(function (tag) {

                var tagDiv = `
                            <div class="tag" style="background-color: ${tag.colorHex}; border-radius: 20px 20px 20px 20px; padding-right: 15px; padding-left:15px; margin-left: 7px;">

                                <input type="hidden" name="Tags[${index}].ColorHex" value="${tag.colorHex}">
                                <input type="hidden" name="Tags[${index}].Name" value="${tag.name}">
                                <input type="hidden" name="Tags[${index}].Id" value="${tag.id}">

                                <label style="font-weight: bold">${tag.name}</label>
                            </div>`;

                $(".tags").append(tagDiv);

                ++index;
            });

            $(".tags").append(`
                             <button id="add-button" style="margin-left: 7px">
                                <svg id="add-button-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                                    <path d="M256 80c0-17.7-14.3-32-32-32s-32 14.3-32 32V224H48c-17.7 0-32 14.3-32 32s14.3 32 32 32H192V432c0 17.7 14.3 32 32 32s32-14.3 32-32V288H400c17.7 0 32-14.3 32-32s-14.3-32-32-32H256V80z" />
                                </svg>
                            </button>`);
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function addLink(ticketId) {

    var linkDataDiv = `
            <div id="link-data-div">
                <div style="position: relative">
                    <input id="link-name" type="text" class="input" name="Name" placeholder=""/>
                    <label class="placeholder">Название</label>
                </div>
                <div style="position: relative; margin-left: 10px;">
                    <input id="link-source" type="text" class="input" name="Source" placeholder=""/>
                    <label class="placeholder">Ссылка</label>
                </div>
                <button id="accept-new-link" type="button" onclick="acceptNewLink(${ticketId})">
                        <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" fill="#15c118">
                            <path d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z" />
                        </svg>
                    </button>

                    <button id="cancel-new-link" type="button" onclick="cancelNewLink()">
                        <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 384 512" fill="#f01313">
                            <path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z" />
                        </svg>
                    </button>
            </div>`;

    $("#links").find(".inner-tab-content").prepend(linkDataDiv);

    $("#links-empty").remove();
}

function cancelNewLink() {
    $("#link-data-div").remove();

    var linksDiv = $("#links").find(".inner-tab-content");

    if (linksDiv.hasClass("ticket-link") != -1) {
        linksDiv.prepend(`<h4 id="links-empty"style="color: #d8d8d8">Нет ссылок в карточке</h4>`);
    }
}

function acceptNewLink(ticketId) {

    var link = {
        Name: $("#link-name").val(),
        Source: $("#link-source").val()
    };

    $.ajax({
        url: "/Link/CreateLink",
        method: "POST",
        data: { ticketId, link },
        success: function (result) {
            $("#links").find(".inner-tab-content").empty();

            result.forEach(function (link) {
                var linkDiv = `
                        <div id="link-${link.id}" class="ticket-link">
                            <a class="link-label" href="${link.source}">${link.name}</a>
                        </div>`;

                $("#links").find(".inner-tab-content").append(linkDiv);
            });
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function delLink(linkId) {
    $.ajax({
        url: "/Link/DeleteLink",
        method: "POST",
        data: { linkId },
        success: function (result) {
            $("#links").find(".inner-tab-content").find("#link-" + linkId).remove();
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

var linkName;
var linkSource;

function editLink(ticketId, linkId) {

    linkName = $("#link-" + linkId).find(".link-label").text();
    linkSource = $("#link-" + linkId).find(".link-label").attr("href");

    var linkDataDiv = `
            <div id="link-data-div">
                <div style="position: relative">
                    <input id="link-name" type="text" class="input" name="Name" placeholder="" value="${linkName}"/>
                    <label class="placeholder">Название</label>
                </div>
                <div style="position: relative; margin-left: 10px;">
                    <input id="link-source" type="text" class="input" name="Source" placeholder="" value="${linkSource}"/>
                    <label class="placeholder">Ссылка</label>
                </div>
                <button id="accept-new-link" type="button" onclick="acceptEditLink(${ticketId}, ${linkId})">
                        <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" fill="#15c118">
                            <path d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z" />
                        </svg>
                    </button>

                    <button id="cancel-new-link" type="button" onclick="cancelEditLink(${ticketId}, ${linkId})">
                        <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 384 512" fill="#f01313">
                            <path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z" />
                        </svg>
                    </button>
            </div>`;

    $("#link-" + linkId).html(linkDataDiv);
}

function cancelEditLink(ticketId, linkId) {

    $("#link-data-div").remove();

    var linkDiv = `
            <a class="link-label" href="${linkSource}">${linkName}</a>
            <button id="del-link" type="button" onclick="delLink(${linkId})">
                <svg class="delete-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                    <path d="M432 256c0 17.7-14.3 32-32 32L48 288c-17.7 0-32-14.3-32-32s14.3-32 32-32l352 0c17.7 0 32 14.3 32 32z" />
                </svg>
            </button>
            <button id="edit-link" type="button" onclick="editLink(${ticketId}, ${linkId})">
                <svg id="edit-board-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                    <path d="M362.7 19.3L314.3 67.7 444.3 197.7l48.4-48.4c25-25 25-65.5 0-90.5L453.3 19.3c-25-25-65.5-25-90.5 0zm-71 71L58.6 323.5c-10.4 10.4-18 23.3-22.2 37.4L1 481.2C-1.5 489.7 .8 498.8 7 505s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L421.7 220.3 291.7 90.3z" />
                </svg>
            </button>`;

    $("#link-" + linkId).html(linkDiv);

}

function acceptEditLink(ticketId, linkId) {

    //linkName = $("#link-" + linkId).find(".link-label").text();
    //linkSource = $("#link-" + linkId).find(".link-label").attr("href");
    linkName = $("#link-name").val();
    linkSource = $("#link-source").val();
    $("#link-data-div").remove();

    var link = {
        Id: linkId,
        Name: linkName,
        Source: linkSource
    };

    $.ajax({
        url: "/Link/EditLink",
        method: "POST",
        data: { newLinkData: link },
        success: function (result) {
            $("#link-" + result.id).html(`
                    <a class="link-label" href="${result.source}">${result.name}</a>
                    <button id="del-link" type="button" onclick="delLink(${linkId})">
                        <svg class="delete-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                            <path d="M432 256c0 17.7-14.3 32-32 32L48 288c-17.7 0-32-14.3-32-32s14.3-32 32-32l352 0c17.7 0 32 14.3 32 32z" />
                        </svg>
                    </button>
                    <button id="edit-link" type="button" onclick="editLink(${ticketId}, ${result.id})">
                        <svg id="edit-board-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                            <path d="M362.7 19.3L314.3 67.7 444.3 197.7l48.4-48.4c25-25 25-65.5 0-90.5L453.3 19.3c-25-25-65.5-25-90.5 0zm-71 71L58.6 323.5c-10.4 10.4-18 23.3-22.2 37.4L1 481.2C-1.5 489.7 .8 498.8 7 505s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L421.7 220.3 291.7 90.3z" />
                        </svg>
                    </button>`);
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function uploadFile(ticketId) {
    var fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.style.display = 'none';
    fileInput.multiple = true;

    document.body.appendChild(fileInput);

    fileInput.addEventListener('change', function () {
        // Получаем выбранные файлы
        var files = fileInput.files;

        // Создаем объект FormData и добавляем все файлы
        var formData = new FormData();

        formData.append('ticketId', ticketId);
        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }

        // Создаем объект XMLHttpRequest
        var xhr = new XMLHttpRequest();

        // Настраиваем запрос
        xhr.open('POST', '/File/AddFiles', true);

        // Обработка завершения запроса
        xhr.onload = function () {
            if (xhr.status === 200) {

                $("#files-empty").remove();
                var files = JSON.parse(xhr.responseText);

                $("#files").find(".inner-tab-content").empty();

                files.forEach(function (file) {

                    var fileDiv = `
                        <div id="file-${file.id}" class="ticket-file">
                            <label class="file-label" onclick="downloadFile(${file.id}, '${file.name}')">${file.name}</label>
                            <button id="del-link" type="button" onclick="delFile(${file.id})">
                                <svg class="delete-image" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                                    <path d="M432 256c0 17.7-14.3 32-32 32L48 288c-17.7 0-32-14.3-32-32s14.3-32 32-32l352 0c17.7 0 32 14.3 32 32z" />
                                </svg>
                            </button>
                        </div>`;

                    $("#files").find(".inner-tab-content").append(fileDiv);
                });

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


function delFile(fileId) {

    $.ajax({
        url: "/File/DeleteFile",
        method: "POST",
        data: { fileId },
        success: function (result) {
            $("#files").find(".inner-tab-content").find("#file-" + fileId).remove();
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function downloadFile(fileId, fileName) {
    // Создаем объект XMLHttpRequest
    var xhr = new XMLHttpRequest();

    // Настраиваем запрос
    xhr.open('GET', '/File/DownloadFile?fileId=' + fileId + '&fileName=' + encodeURIComponent(fileName), true);

    // Ответ будет в виде массива байтов
    xhr.responseType = 'arraybuffer';

    // Обработка завершения запроса
    xhr.onload = function () {
        if (xhr.status === 200) {
            // Создаем ссылку для скачивания
            var blob = new Blob([xhr.response], { type: 'application/octet-stream' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);

            // Используем название файла для сохранения
            link.download = fileName;

            // Добавляем ссылку на страницу и эмулируем клик
            document.body.appendChild(link);
            link.click();

            // Удаляем ссылку
            document.body.removeChild(link);
        } else {
            console.error('Произошла ошибка:', xhr.status, xhr.statusText);
            toastr.error('Произошла ошибка при скачивании файла.');
        }
    };

    // Отправляем запрос
    xhr.send();
}

function removePart(ticketId, partId) {
    $.ajax({
        url: "/Ticket/RemoveParticipant",
        method: "POST",
        data: { ticketId: ticketId, participantId: partId },
        success: function () {

            $("#part-" + partId).remove();

            if ($("#participants").find(".inner-tab-content").hasClass("ticket-part") != -1) {
                $("#participants").find(".inner-tab-content").append(`<h4 style="color: #d8d8d8">Нет участников в карточке</h4>`);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function addExecutor(ticketId, userId) {
    $.ajax({
        url: "/Ticket/AddExecutor",
        method: "POST",
        data: { ticketId: ticketId, userId: userId },
        success: function () {
            $("#ticket-executor")
                .html(`<label class="info-card-author" style="margin-left: auto;">Отвественнный: <a href="#" style="color: #d8d8d8">${result.username}</a></label>`);
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function sendComment(ticketId) {
    var content = $(".comment-enter").val();

    $.ajax({
        url: "/Comment/AddComment",
        method: "POST",
        data: { ticketId: ticketId, content: content },
        success: function (result) {
            console.log(result);
            var commentDiv = `
                        <div class="comment-content">
                            <div class="comment-info">
                                <label>${result.authorName}</label>
                                    <label class="comment-creation-date">${result.creationDate}</label>
                             </div>
                            <div class="comment-text">${result.content}</div>
                        </div>`;

            $("#comments-empty").remove();

            $(".comments-div").prepend(commentDiv);
            $(".comment-enter").val("");
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

function changeStatus(ticketId, boardId) {
    $.ajax({
        url: "/Ticket/ChangeStatus",
        method: "POST",
        data: { ticketId, boardId },
        success: function (result) {

            var modal = document.getElementById('myModalTicket');
            var form = modal.querySelector('form');
            form.querySelector('input[name="StatusId"]').value = boardId;

            var ticketsDiv = $('#board-' + boardId);
            ticketsDiv.empty();

            $("#ticket-" + ticketId).remove();

            result.tickets.forEach(function (ticket) {
                var ticketElement = `
                                    <div class="ticket" id="ticket-${ticket.id}" onclick="openTicket(${ticket.workspaceId}, ${ticket.id})" style="display: block;">
                                    <div class="ticket-title-preview">
                                        <label id="ticket-preview-label">${ticket.title}</label>
                                        <button id="ticket-del-button" onclick="deleteTicket(${ticket.statusId}, ${ticket.id})">
                                            <svg xmlns="http://www.w3.org/2000/svg" height="16" width="14" viewBox="0 0 448 512" fill="red">
                                                <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"/>
                                            </svg>
                                        </button>
                                    </div>
                                `;
                if (ticket.priority != null) {
                    ticketElement += `
                                <div class="ticket-preview-priority">
                                    <svg class="prio-image" width="20px" height="20px" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg">
                                        <path fill="#d8d8d8" d="M288 128h608L736 384l160 256H288v320h-96V64h96v64z" />
                                    </svg>
                                    <div style="margin-left: 7px; padding-left: 7px; padding-right: 7px; background-color: ${ticket.priority.colorHex}; border-radius: 20px;">
                                        <label style="font-weight: bold; color: black">${ticket.priority.name}</label>
                                    </div>
                                </div>`;
                }

                ticketElement += `</div>`;
                // Добавляем элемент в ticketsDiv
                ticketsDiv.append(ticketElement);
            });

            $(".dropdown-status-content").empty();

            result.statuses.forEach(function (status) {
                $(".dropdown-status-content")
                    .append(`<button type="button" onclick="changeStatus(${ticketId}, ${status.id})">${status.name}</button>`);
            });

            $(".info-status").text("Статус: " + result.currentStatusName);

            toastr.success('Статус успешно изменен!');
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText || 'Произошла ошибка!');
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    var ticketContainers = [];
    var fuse;

    function updateListForSearch() {
        var ticketsData = [];

        // Получаем все элементы с классом ticket-preview-label
        var ticketLabels = document.querySelectorAll("#ticket-preview-label");

        // Проходимся по каждому элементу и добавляем данные в массив ticketsData
        ticketLabels.forEach(function (label) {
            var ticketId = label.closest(".ticket").id.replace("ticket-", ""); // Получаем ID тикета
            var ticketTitle = label.textContent; // Получаем название тикета

            // Добавляем данные в массив ticketsData
            ticketsData.push({
                Id: ticketId,
                Title: ticketTitle
                // Добавьте другие свойства тикета по необходимости
            });
        });

        // Инициализируем fuse.js с настройками (подробнее см. документацию fuse.js)
        var fuseOptions = {
            keys: ["Title"],
            includeScore: true,
            threshold: 0.5 // Подстройте этот параметр в зависимости от ваших требований
        };
        fuse = new Fuse(ticketsData, fuseOptions);

        // Получаем элементы DOM
        ticketContainers = document.querySelectorAll(".ticket");
    }

    // Функция для выполнения поиска и обновления отображения
    function performSearch(query) {

        if (query.trim() === "") {
            ticketContainers.forEach(function (ticketContainer) {
                ticketContainer.style.display = "block";
            });

            return;
        }

        var result = fuse.search(query);

        // Отображаем только те тикеты, которые соответствуют результатам поиска
        ticketContainers.forEach(function (ticketContainer) {
            var ticketId = ticketContainer.id.replace("ticket-", "");
            var foundTicket = result.find(function (item) {
                return item.item.Id === ticketId;
            });

            // Отображаем или скрываем тикет в зависимости от результата поиска
            if (foundTicket) {
                ticketContainer.style.display = "block";
            } else {
                ticketContainer.style.display = "none";
            }
        });
    }

    // Вызываем функцию поиска при вводе в поле поиска
    var searchInput = document.getElementById("search-input");
    searchInput.addEventListener("input", function () {
        var searchQuery = searchInput.value;
        updateListForSearch();
        performSearch(searchQuery);
    });
});

function checkTicketChanges() {
    var title = $("#title-cache-value").val();
    var description = $("#description-cache-value").val();
    var realDescription = document.querySelector('.description-text-container').innerHTML;

    console.log("title: " + title);
    console.log("descr: " + description);
    console.log("realTitle: " + $(".ticket-title").val());
    console.log("realDescr: " + realDescription);

    if (realDescription.trim() == description.trim() &&
        $(".ticket-title").val() == title) {

        updateTicket();

        document.getElementById('myModalTicket').style.display = 'none';
        document.getElementById('overlayTicket').style.display = 'none';
        var currentUrl = window.location.href;

        // Регулярное выражение для удаления последнего сегмента URL
        var regex = /\/\d+$/;

        // Обрезаем URL до необходимого уровня
        var trimmedUrl = currentUrl.replace(regex, '');

        // Заменяем текущее состояние истории новым состоянием с обновленным URL
        history.replaceState(null, null, trimmedUrl);


    }
    else {
        closeModalTicket();
    }
}

function updateTicket() {
    var modal = document.getElementById('myModalTicket');
    var form = modal.querySelector('form');
    var ticketId = form.querySelector('input[name="Id"]').value;

    var prio = form.querySelector('input[name="Priority.Id"]');

    if (prio == null) {
        return;
    }

    var priority = {
        Id: form.querySelector('input[name="Priority.Id"]').value,
        Name: form.querySelector('input[name="Priority.Name"]').value,
        Color: form.querySelector('input[name="Priority.ColorHex"]').value
    };

    $("#ticket-" + ticketId).find(".ticket-preview-priority").remove();

    $("#ticket-" + ticketId).append(`
                                <div class="ticket-preview-priority">
                                    <svg class="prio-image" width="20px" height="20px" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg">
                                        <path fill="#d8d8d8" d="M288 128h608L736 384l160 256H288v320h-96V64h96v64z" />
                                    </svg>
                                    <div style="margin-left: 7px; padding-left: 7px; padding-right: 7px; background-color: ${priority.Color}; border-radius: 20px;">
                                        <label style="font-weight: bold; color: black">${priority.Name}</label>
                                    </div>
                                </div>`);

}