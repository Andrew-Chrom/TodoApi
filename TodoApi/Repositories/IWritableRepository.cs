using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories
{
    public interface IWritableRepository
    {
        public Task<long> CreateTodo(TodoItem item);
        public Task UpdateTodo(TodoItemCreateDTO dto, long id, string userId);
        public Task DeleteTodo(long id, string userId);
    }
}
