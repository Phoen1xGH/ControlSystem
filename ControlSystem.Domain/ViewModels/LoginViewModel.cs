using System.ComponentModel.DataAnnotations;

namespace ControlSystem.Domain.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Логин не должен быть пустым")]
        [MinLength(3, ErrorMessage = "Имя должно содержать 3 символа и более")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пароль не должен быть пустым")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
