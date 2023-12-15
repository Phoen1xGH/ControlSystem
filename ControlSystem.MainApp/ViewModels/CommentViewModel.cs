using ControlSystem.Domain.Entities;

namespace ControlSystem.MainApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string Content { get; set; }


        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            AuthorName = comment.Author.Username;
            Content = comment.Content;
        }
    }
}
