using ControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlSystem.DAL
{
    public class ControlSystemContext : DbContext
    {
        public ControlSystemContext(DbContextOptions<ControlSystemContext> options)
            : base(options)
        {

        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Chart> Charts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FileAttachment> Attachments { get; set; }
        public DbSet<FileContent> FilesContent { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
    }
}
