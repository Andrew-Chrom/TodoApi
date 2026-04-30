using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoItems
{
    public class ReadonlyTodoItemRepository : IReadonlyTodoItemRepository
    {
        private readonly QueryDbContext _ctx;
        public ReadonlyTodoItemRepository(QueryDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<TodoItemResponseDTO>> GetTodos(string userId, bool? isComplete)
        {
            var todos = _ctx.TodoItems
                .Select(todo => new TodoItemResponseDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete,
                TodoListId = todo.TodoListId,
                UserId = todo.UserId
            })
                .Where(x => x.UserId == userId);

            if (isComplete != null)
                todos = todos.Where(x => x.IsComplete == isComplete);

            return await todos.ToListAsync();
        }
        public async Task<TodoItemResponseDTO?> GetTodoById(long id, string userId)
        {
            
            var todoItem = await _ctx.TodoItems
                .Select(todo => new TodoItemResponseDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete,
                TodoListId = todo.TodoListId,
                UserId = todo.UserId
            })
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return todoItem;
        }
    }
}
