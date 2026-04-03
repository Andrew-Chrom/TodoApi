using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.Query
{
    public record GetTodoListByIdQuery(long Id, string UserId);
    public class GetTodoListByIdHandler
    {
        public readonly IReadonlyTodoListRepository _repository;
        public GetTodoListByIdHandler(IReadonlyTodoListRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoListItemResponseDto> Handle(GetTodoListByIdQuery query)
        {
            return await _repository.GetTodoListById(query.Id, query.UserId);
        }

    }
}
