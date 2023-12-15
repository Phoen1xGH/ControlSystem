using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlSystem.DAL.Repositories
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly ControlSystemContext _context;

        public TicketRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Ticket entity)
        {
            await _context.Tickets.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Ticket entity)
        {
            _context.Tickets.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Ticket> GetAll()
        {
            return _context.Tickets
                .Include(x => x.Author)
                .Include(x => x.Participants)
                .Include(x => x.Tags)
                .Include(x => x.Links)
                .Include(x => x.Attachments)
                    .ThenInclude(x => x.FileContent)
                .Include(x => x.Comments)
                .Include(x => x.Status);
        }

        public IQueryable<Ticket> GetAllWithoutFiles()
        {
            return _context.Tickets
                .Include(x => x.Participants)
                .Include(x => x.Tags)
                .Include(x => x.Links)
                .Include(x => x.Attachments)
                .Include(x => x.Comments);
        }

        public async Task Update(Ticket entity)
        {
            _context.Tickets.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
