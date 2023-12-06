using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;

namespace ControlSystem.DAL.Repositories
{
    public class TagsRepository : IRepository<Tag>
    {
        private readonly ControlSystemContext _context;

        public TagsRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Tag entity)
        {
            _context.Tags.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Tag> GetAll()
        {
            return _context.Tags;
        }

        public async Task Update(Tag entity)
        {
            _context.Tags.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
