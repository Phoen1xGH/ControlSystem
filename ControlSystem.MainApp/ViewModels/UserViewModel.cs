using ControlSystem.Domain.Entities;

namespace ControlSystem.MainApp.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public UserViewModel(UserAccount user)
        {
            Id = user.Id;
            Name = user.Username;
        }
    }
}
