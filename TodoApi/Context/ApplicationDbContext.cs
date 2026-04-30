using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Context
{

    
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected ApplicationDbContext(DbContextOptions options) : base(options) { }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "API";
                        entry.Entity.Created = DateTime.UtcNow;
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "API";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "API";
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.NormalizedEmail)
                .HasDatabaseName("EmailIndex")
                .IsUnique();
        }
    }

}
