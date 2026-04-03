using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoItems
{
    public interface IReadonlyTodoItemRepository
    {
        Task<List<TodoItem>> GetTodos(string userId, bool? isComplete);
        Task<TodoItem> GetTodoById(long id, string userId);
    }
}
