using System.ComponentModel.DataAnnotations;

namespace ControlSystem.Domain.ViewModels
{
    internal class ChartViewModel
    {
        [Required(ErrorMessage = "Введите название для диграммы бизнес-процесса")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Отсутствуют данные диаграммы")]
        public string Xml { get; set; }
    }
}
