using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
