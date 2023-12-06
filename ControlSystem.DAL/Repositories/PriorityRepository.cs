using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;

namespace ControlSystem.DAL.Repositories
{
    public class PriorityRepository : IRepository<Priority>
    {
        private readonly ControlSystemContext _context;

        public PriorityRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Priority entity)
        {
            await _context.Priorities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Priority entity)
        {
            _context.Priorities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Priority> GetAll()
        {
            return _context.Priorities;
        }

        public async Task Update(Priority entity)
        {
            _context.Priorities.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
