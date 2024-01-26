using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;

namespace ControlSystem.DAL.Repositories
{
    public class UpdatesRepository : IRepository<UpdateInfo>
    {
        private readonly ControlSystemContext _context;

        public UpdatesRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(UpdateInfo entity)
        {
            await _context.UpdatesInfo.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(UpdateInfo entity)
        {
            _context.UpdatesInfo.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<UpdateInfo> GetAll()
        {
            return _context.UpdatesInfo;
        }

        public async Task Update(UpdateInfo entity)
        {
            _context.UpdatesInfo.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
