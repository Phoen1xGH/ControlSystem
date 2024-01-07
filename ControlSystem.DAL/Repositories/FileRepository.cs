using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlSystem.DAL.Repositories
{
    public class FileRepository : IRepository<FileAttachment>
    {
        private readonly ControlSystemContext _context;

        public FileRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(FileAttachment entity)
        {
            await _context.Attachments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(FileAttachment entity)
        {
            _context.Attachments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<FileAttachment> GetAll()
        {
            return _context.Attachments;
        }
        public IQueryable<FileAttachment> GetAllWithContent()
        {
            return _context.Attachments.Include(x => x.FileContent);
        }

        public async Task Update(FileAttachment entity)
        {
            _context.Attachments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
