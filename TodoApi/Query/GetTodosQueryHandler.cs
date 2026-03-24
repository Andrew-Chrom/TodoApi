using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Query
{
    public record GetTodosQuery(string UserId, bool? IsComplete);

    public class GetTodosQueryHandler
    {
        public async Task<List<TodoItem>> Handle(
        GetTodosQuery query,
        ApplicationDbContext db)
        {
            var todos = db.TodoItems.Where(x => x.UserId == query.UserId);

            if (query.IsComplete != null)
                todos = todos.Where(x => x.IsComplete == query.IsComplete);

            return await todos.ToListAsync();
        }

    }
}
