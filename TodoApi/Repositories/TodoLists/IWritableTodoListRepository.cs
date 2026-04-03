using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoLists
{
    public interface IWritableTodoListRepository
    {
        public Task<TodoList> GetTodoListById(long? id, string userId);
        public Task<long> CreateTodoList(TodoListCreateDto dto, string userId);
        public Task UpdateTodoList(TodoListCreateDto dto, long id, string userId);
        public Task DeleteTodoList(long id, string userId);
    }
}
