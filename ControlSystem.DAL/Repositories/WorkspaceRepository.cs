using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlSystem.DAL.Repositories
{
    public class WorkspaceRepository : IRepository<Workspace>
    {
        private readonly ControlSystemContext _context;

        public WorkspaceRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(Workspace entity)
        {
            await _context.Workspaces.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Workspace entity)
        {
            _context.Workspaces.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Workspace> GetAll()
        {
            return _context.Workspaces
                .Include(x => x.Participants)
                .Include(x => x.Boards);
        }

        public async Task Update(Workspace entity)
        {
            _context.Workspaces.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddBoard(Workspace workspace, Board board)
        {
            workspace.Boards.Add(board);
            await _context.SaveChangesAsync();
        }
    }
}
