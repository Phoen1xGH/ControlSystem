using System.ComponentModel.DataAnnotations;

namespace ControlSystem.Domain.ViewModels
{
    public class BoardViewModel
    {
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Выберете цвет")]
        public string ColorHex { get; set; }
    }
}
