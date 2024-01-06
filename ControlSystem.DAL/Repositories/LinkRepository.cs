using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;

namespace ControlSystem.DAL.Repositories
{
    public class LinkRepository : IRepository<Link>
    {
        private readonly ControlSystemContext _context;

        public LinkRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Link entity)
        {
            await _context.Links.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Link entity)
        {
            _context.Links.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Link> GetAll()
        {
            return _context.Links;
        }

        public async Task Update(Link entity)
        {
            _context.Links.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
