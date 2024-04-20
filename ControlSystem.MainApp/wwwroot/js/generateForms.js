document.addEventListener("DOMContentLoaded", function () {
    const dynamicFieldsContainer = document.getElementById("dynamic-fields-container");
    const addFieldButton = document.getElementById("add-field-button");
    let fieldCount = 0;

    let taskCount = 0;/*document.querySelector('[name="taskCount"]').value;*/
    let exGateCount = 0;/*document.querySelector('[name="exGateCount"]').value;*/
    let endCount = 0;/*document.querySelector('[name="endCount"]').value;*/

    addFieldButton.addEventListener("click", function () {
        fieldCount++;

        const newField = document.createElement("div");
        newField.innerHTML = `
                        <select class="select-list" name="element-type-${fieldCount}">
                            <option value="">Выберите тип элемента</option>
                            <option value="Participants">Отвечающий за процесс</option>
                            <option value="StartEvent">Начальное событие</option>
                            <option value="Task">Задача</option>
                            <option value="ExclusiveGateway">Условие</option>
                            <option value="EndEvent">Конечное событие</option>
                        </select>
                        <div class="element-fields" id="element-fields-${fieldCount}"></div>
                    `;
        newField.className = "choice-div";
        dynamicFieldsContainer.appendChild(newField);

        const elementFieldsContainer = newField.querySelector(`#element-fields-${fieldCount}`);
        const elementTypeSelect = newField.querySelector(`select[name="element-type-${fieldCount}"]`);

        elementTypeSelect.addEventListener("change", function () {
            const selectedElementType = elementTypeSelect.value;
            elementFieldsContainer.innerHTML = "";

            if (selectedElementType === "Participants") {
                elementFieldsContainer.innerHTML = `
                                <p>
                                    <div style="position: relative">
                                        <input class="name-input" type="text" name="ParticipantName" placeholder=""/>
                                        <label class="placeholder">Название</label>
                                    </div>
                                </p>
                            `;
            }
            else if (selectedElementType === "StartEvent") {
                elementFieldsContainer.innerHTML = `
                        <p>
                            <div class="flexible">
                                <div>
                                    <div style="position: relative">
                                        <input class="name-input" type="text" name="StartEvent.Name" placeholder=""/>
                                        <label class="placeholder">Название</label>
                                    </div>
                                </div>
                                <div>
                                    <div style="position: relative; margin-left: 10px">
                                        <input class="name-input" type="text" name="StartEvent.Outgoing" placeholder=""/>
                                        <label class="placeholder">Выход</label>
                                    </div>
                                </div>
                           </div>
                        </p>`;
            }
            else if (selectedElementType === "Task") {
                const taskIndex = taskCount++;

                elementFieldsContainer.innerHTML = `
                            <p>
                                <div class="flexible">
                                    <div style="position: relative;">
                                        <input class="name-input" type="text" name="Task.Incoming" placeholder=""/>
                                        <label class="placeholder">Вход</label>
                                    </div>
                                    <div style="position: relative; margin-left: 10px">
                                        <input type="text" class="name-input" name="Task.Name" placeholder=""/>
                                        <label class="placeholder">Название</label>
                                    </div>
                                    <div style="position: relative; margin-left: 10px">
                                        <input class="name-input" type="text" name="Task.Outgoing" placeholder=""/>
                                        <label class="placeholder">Выход</label>
                                    </div>
                                </div>
                            </p>`;

            }
            else if (selectedElementType === "ExclusiveGateway") {
                const exGateIndex = exGateCount++;

                elementFieldsContainer.innerHTML = `
                            <p>
                                <div class="flexible">
                                    <div style="position: relative; margin: auto 0 auto 0">
                                        <input class="name-input" type="text" name="ExclusiveGateway.Incomings" placeholder=""/>
                                        <label class="placeholder">Вход</label>
                                    </div>
                                    <div style="position: relative; margin: auto 0 auto 10px">
                                        <input type="text" class="name-input" name="ExclusiveGateway.Name" placeholder=""/>
                                        <label class="placeholder">Название</label>
                                    </div>
                                    <div style="display: flex; flex-direction: column">
                                        <div style="position: relative; margin: 0 0 auto 10px;">
                                            <input type="text" class="name-input" name="ExclusiveGateway.OutgoingsY" placeholder=""/>
                                            <label class="placeholder">Условие: Да</label>
                                        </div>
                                        <div style="position: relative; margin: 40px 0 auto 10px;">
                                            <input type="text" class="name-input" name="ExclusiveGateway.OutgoingsN" placeholder=""/>
                                            <label class="placeholder">Условие: Нет</label>
                                        </div>
                                    </div>
                                </div>
                            </p>`;
            }
            else if (selectedElementType === "EndEvent") {
                const endIndex = endCount++;

                elementFieldsContainer.innerHTML = `
                            <p>
                                <div class="flexible">
                                    <div style="position: relative;">
                                        <input class="name-input" type="text" name="EndEvent.Incoming" placeholder="">
                                        <label class="placeholder"l>Вход</label>
                                    </div>
                                    <div style="position: relative; margin-left: 10px">
                                        <input type="text" class="name-input" name="EndEvent.Name" placeholder=""/>
                                        <label class="placeholder">Название</label>
                                    </div>
                                </div>
                            </p>`;
            }
        });

        // Кнопка для удаления поля
        const removeFieldButton = document.createElement("button");
        removeFieldButton.type = "button";
        removeFieldButton.textContent = "Удалить";
        removeFieldButton.className = "remove-button";
        removeFieldButton.addEventListener("click", function () {
            dynamicFieldsContainer.removeChild(newField);
        });

        newField.appendChild(removeFieldButton);
    });
});

let tasks = [];
let ends = [];
let exgates = [];

let allData = {
    Collaboration: {
        Participants: []
    },
    Processes: []
};

function getTasksData() {

    const inputIncoming = document.querySelectorAll('input[name^="Task.Incoming"]');
    const inputNames = document.querySelectorAll('input[name^="Task.Name"]');
    const inputOutgoing = document.querySelectorAll('input[name^="Task.Outgoing"]');

    if (inputIncoming.length === inputNames.length && inputNames.length === inputOutgoing.length) {
        for (let i = 0; i < inputIncoming.length; i++) {
            tasks.push({
                Incoming: inputIncoming[i].value,
                Name: inputNames[i].value,
                Outgoing: inputOutgoing[i].value
            });
        }
    }
}

function getEndEventsData() {
    const inputIncoming = document.querySelectorAll('input[name^="EndEvent.Incoming"]');
    const inputNames = document.querySelectorAll('input[name^="EndEvent.Name"]');

    if (inputIncoming.length === inputNames.length) {
        for (let i = 0; i < inputIncoming.length; i++) {
            ends.push({
                Incoming: inputIncoming[i].value,
                Name: inputNames[i].value,
            });
        }
    }
}

function getExclusiveGatewaysData() {
    const inputIncoming = document.querySelectorAll('input[name^="ExclusiveGateway.Incomings"]');
    const inputNames = document.querySelectorAll('input[name^="ExclusiveGateway.Name"]');
    const inputOutgoingY = document.querySelectorAll('input[name^="ExclusiveGateway.OutgoingsY"]');
    const inputOutgoingN = document.querySelectorAll('input[name^="ExclusiveGateway.OutgoingsN"]');

    if (inputIncoming.length === inputNames.length) {
        for (let i = 0; i < inputIncoming.length; i++) {
            const incomingList = [];
            const outgoingList = [];

            incomingList.push(inputIncoming[i].value);
            // Добавляем значения в порядке: OutgoingY, затем OutgoingN
            outgoingList.push(inputOutgoingY[i].value);
            outgoingList.push(inputOutgoingN[i].value);

            exgates.push({
                Incomings: incomingList,
                Name: inputNames[i].value,
                Outgoings: outgoingList
            });
        }
    }
}

function getAllData() {
    getTasksData();
    getExclusiveGatewaysData();
    getEndEventsData();
    let participant = document.querySelector('input[name^="ParticipantName"]').value;
    let startName = document.querySelector('input[name^="StartEvent.Name"]').value;
    let startOut = document.querySelector('input[name^="StartEvent.Outgoing"]').value;

    let startEvent = {
        Name: startName,
        Outgoing: startOut
    }

    let processes = {
        TaskList: [],
        StartEvent: null,
        EndEventList: [],
        ExclusiveGatewayList: []
    }

    processes.StartEvent = startEvent;
    processes.TaskList = tasks;
    processes.ExclusiveGatewayList = exgates;
    processes.EndEventList = ends;

    let participantEl = {
        Name: participant
    }
    allData.Collaboration.Participants.push(participantEl);
    // allData.Collaboration = collaboration;
    allData.Processes.push(processes);
}

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("submitButton").addEventListener("click", function () {

        getAllData();

        // Преобразование списка в JSON строку
        var jsonData = JSON.stringify(allData);

        // Заполнение скрытого поля формы
        document.getElementById("allData").value = jsonData;
    });
});