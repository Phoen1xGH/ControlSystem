using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;

namespace ControlSystem.DAL.Repositories
{
    public class ChartRepository : IRepository<Chart>
    {
        private readonly ControlSystemContext _context;

        public ChartRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Chart entity)
        {
            await _context.Charts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Chart entity)
        {
            _context.Charts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Chart> GetAll()
        {
            return _context.Charts;
        }

        public async Task Update(Chart entity)
        {
            _context.Charts.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
