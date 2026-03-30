using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IReadonlyRepository
    {
        Task<List<TodoItem>> GetTodos(string userId, bool? isComplete);
        Task<TodoItem> GetTodoById(long id, string userId);
    }
}
