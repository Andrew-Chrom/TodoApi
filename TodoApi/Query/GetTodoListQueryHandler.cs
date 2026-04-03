using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.Query
{
    public record GetTodoListQuery(string UserId);
    public class GetTodoListQueryHandler
    {
        public readonly IReadonlyTodoListRepository _repository;
        public GetTodoListQueryHandler(IReadonlyTodoListRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<TodoListResponseDto>> Handle(GetTodoListQuery query)
        {
            return await _repository.GetTodoList(query.UserId);
        }
    }
}
