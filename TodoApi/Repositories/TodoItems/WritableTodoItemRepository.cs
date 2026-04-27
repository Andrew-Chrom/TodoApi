using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Context;

namespace TodoApi.Repositories.TodoItems
{
    public class WritableTodoItemRepository : IWritableTodoItemRepository
    {
        private readonly CommandDbContext _ctx;
        public WritableTodoItemRepository(CommandDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<TodoItemResponseDTO?> GetTodoById(long id, string userId)
        {
            var todoItem = await _ctx.TodoItems
                .Where(x => x.Id == id && x.UserId == userId)
                .Select(x => new TodoItemResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsComplete = x.IsComplete,
                    TodoListId = x.TodoListId,
                })
                .FirstOrDefaultAsync();

            return todoItem;

        }
        public async Task<long> CreateTodo(TodoItem item)
        {
            _ctx.TodoItems.Add(item);
            await _ctx.SaveChangesAsync();
            
            return item.Id;
        }
        public async Task UpdateTodo(TodoItemCreateDTO dto, long id, string userId)
        {

            var todoItem = await _ctx.TodoItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            todoItem.Name = dto.Name;
            todoItem.IsComplete = dto.IsComplete;
            todoItem.TodoListId = dto.TodoListId;
        }
        public async Task DeleteTodo(long id, string userId)
        {
            var todoItem = await _ctx.TodoItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            _ctx.TodoItems.Remove(todoItem);
        }
    }
}
