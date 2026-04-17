using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class CommandDbContext : ApplicationDbContext
    {
        public CommandDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
    }
}