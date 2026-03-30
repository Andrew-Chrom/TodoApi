using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class QueryDbContext : ApplicationDbContext
    {
        public QueryDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
