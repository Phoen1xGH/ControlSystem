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
                .Include(x => x.Tickets).ThenInclude(x => x.Author)
                .Include(x => x.Tickets).ThenInclude(x => x.Priority)
                .Include(x => x.Tickets).ThenInclude(x => x.Tags)
                .Include(x => x.Workspace).ThenInclude(x => x.Participants);
        }

        public async Task Update(Board entity)
        {
            _context.Boards.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddTicket(Board board, Ticket ticket)
        {
            board.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
