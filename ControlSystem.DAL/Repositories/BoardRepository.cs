using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlSystem.DAL.Repositories
{
    public class BoardRepository : IRepository<Board>
    {
        private readonly ControlSystemContext _context;
        public BoardRepository(ControlSystemContext context)
        {
            _context = context;
        }
        public async Task Create(Board entity)
        {
            await _context.Boards.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Board entity)
        {
            _context.Boards.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Board> GetAll()
        {
            return _context.Boards
                .Include(x => x.Tickets)
                .Include(x => x.Workspace);
        }

        public async Task Update(Board entity)
        {
            _context.Boards.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
