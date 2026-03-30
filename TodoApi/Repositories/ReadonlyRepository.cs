using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TodoApi.Repositories
{
    public class ReadonlyRepository : IReadonlyRepository
    {
        public readonly QueryDbContext _ctx;
        public ReadonlyRepository(QueryDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<TodoItem>> GetTodos(string userId, bool? isComplete)
        {
            var todos = _ctx.TodoItems.Where(x => x.UserId == userId);

            if (isComplete != null)
                todos = todos.Where(x => x.IsComplete == isComplete);

            return await todos.ToListAsync();
        }
        public async Task<TodoItem> GetTodoById(long id, string userId)
        {
            var todoItem = await _ctx.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            return todoItem;
        }

    }
}
