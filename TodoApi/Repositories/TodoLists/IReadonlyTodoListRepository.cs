using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoLists
{
    public interface IReadonlyTodoListRepository
    {
        Task<List<TodoListResponseDto>> GetTodoList(string userId);
        Task<TodoListItemResponseDto?>? GetTodoListById(long id, string userId);
    }
}
