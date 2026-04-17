using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoItems
{
    public interface IWritableTodoItemRepository
    {
        Task<TodoItemResponseDTO> GetTodoById(long id, string userId);
        public Task<long> CreateTodo(TodoItem item);
        public Task UpdateTodo(TodoItemCreateDTO dto, long id, string userId);
        public Task DeleteTodo(long id, string userId);
    }
}
