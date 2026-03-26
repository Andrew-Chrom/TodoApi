using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Query
{
    public record GetTodosQuery(string UserId, bool? IsComplete);

    public class GetTodosQueryHandler
    {
        public readonly IReadonlyRepository _repository;
        public GetTodosQueryHandler(IReadonlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TodoItem>> Handle(GetTodosQuery query)
        {
            return await _repository.GetTodos(query.UserId, query.IsComplete);
        }

    }
}
