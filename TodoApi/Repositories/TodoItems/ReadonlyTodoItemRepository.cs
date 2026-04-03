using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TodoApi.Repositories.TodoItems
{
    public class ReadonlyTodoItemRepository : IReadonlyTodoItemRepository
    {
        public readonly QueryDbContext _ctx;
        public ReadonlyTodoItemRepository(QueryDbContext ctx)
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

        public async Task<List<TodoList>> GetTodoList(string userId)
        {
            var todoList = _ctx.TodoLists.Where(x => x.UserId == userId);
            return await todoList.ToListAsync();
        }

        public async Task<TodoList> GetTodoListById(long id, string userId)
        {
            var todoList = await _ctx.TodoLists.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
            if(todoList == null)
                throw new NotFoundException("Todo list not found");
            
            return todoList;
        }

    }
}
