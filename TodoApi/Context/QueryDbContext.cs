using Microsoft.EntityFrameworkCore;

namespace TodoApi.Context
{
    public class QueryDbContext : ApplicationDbContext
    {
        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
