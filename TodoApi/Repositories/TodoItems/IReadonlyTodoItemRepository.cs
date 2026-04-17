using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoItems
{
    public interface IReadonlyTodoItemRepository
    {
        Task<List<TodoItemResponseDTO>> GetTodos(string userId, bool? isComplete);
        Task<TodoItemResponseDTO> GetTodoById(long id, string userId);
    }
}
