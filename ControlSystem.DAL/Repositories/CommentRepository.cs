using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;

namespace ControlSystem.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly ControlSystemContext _context;

        public CommentRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Comment entity)
        {
            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public async Task Update(Comment entity)
        {
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
