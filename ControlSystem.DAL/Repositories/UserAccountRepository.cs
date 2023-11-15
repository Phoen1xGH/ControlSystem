using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlSystem.DAL.Repositories
{
    public class UserAccountRepository : IRepository<UserAccount>
    {
        private readonly ControlSystemContext _context;

        public UserAccountRepository(ControlSystemContext context)
        {
            _context = context;
        }

        public async Task Create(UserAccount entity)
        {
            await _context.UserAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(UserAccount entity)
        {
            _context.UserAccounts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<UserAccount> GetAll()
        {
            return _context.UserAccounts
                .Include(x => x.Charts)
                .Include(x => x.Workspaces);
        }

        public async Task Update(UserAccount entity)
        {
            _context.UserAccounts.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task AddChartToUser(UserAccount entity, Chart chart)
        {
            entity.Charts.Add(chart);
            await _context.SaveChangesAsync();
        }
        public async Task AddWorkspaceToUser(UserAccount entity, Workspace workspace)
        {
            entity.Workspaces.Add(workspace);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkspace(UserAccount entity, Workspace workspace)
        {
            entity.Workspaces.Remove(workspace);
            await _context.SaveChangesAsync();
        }
    }
}
