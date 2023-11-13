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

        #endregion

        #region Http statuses
        [Description("Успешно")]
        OK = 200,
        [Description("Внутренняя ошибка сервера")]
        InternalServerError = 500

        #endregion
    }
}
