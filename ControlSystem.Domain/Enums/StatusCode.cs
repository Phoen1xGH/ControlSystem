using System.ComponentModel;

namespace ControlSystem.Domain.Enums
{
    public enum StatusCode
    {
        #region Custom statuses

        [Description("Пользователя с такими данными не существует")]
        UserNotFound = 0,

        [Description("Пользователь с таким логином уже существует")]
        UserAlreadyExists = 1,

        [Description("Рабочее пространство не найдено")]
        WorkspaceNotFound = 2,

        [Description("Доска не найдена")]
        BoardNotFound = 3,

        [Description("Задача не найдена")]
        TicketNotFound = 4,

        [Description("Тег не найден")]
        TagNotFound = 5,

        [Description("Приотритет не найден")]
        PriorityNotFound = 6,

        [Description("Ссылка не найдена")]
        LinkNotFound = 7,

        #endregion

        #region Http statuses
        [Description("Успешно")]
        OK = 200,
        [Description("Внутренняя ошибка сервера")]
        InternalServerError = 500

        #endregion
    }
}
