using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories.TodoItems;

namespace TodoApi.Query
{
    public record GetTodosQuery(string UserId, bool? IsComplete);

    public class GetTodosQueryHandler
    {
        public readonly IReadonlyTodoItemRepository _repository;
        public GetTodosQueryHandler(IReadonlyTodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TodoItemResponseDTO>> Handle(GetTodosQuery query)
        {
            return await _repository.GetTodos(query.UserId, query.IsComplete);
        }

    }
}
