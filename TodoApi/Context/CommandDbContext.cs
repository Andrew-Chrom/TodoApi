using Microsoft.EntityFrameworkCore;

namespace TodoApi.Context
{
    public class CommandDbContext : ApplicationDbContext
    {
        public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
        {
        }
    }
}