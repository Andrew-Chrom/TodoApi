using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories.TodoItems;

namespace TodoApi.Query
{
    public record GetTodoById(long Id, string UserId);

    public class GetTodoByIdHandler
    {
        public readonly IReadonlyTodoItemRepository _repository;
        public GetTodoByIdHandler(IReadonlyTodoItemRepository repository)
        {
            _repository = repository;
        }
        public async Task<TodoItemResponseDTO> Handle(GetTodoById query)
        {
            var todoItem = await _repository.GetTodoById(query.Id, query.UserId);
            if (todoItem == null)
                throw new NotFoundException("TodoItem not found");

            return todoItem;
        }
    }
}
