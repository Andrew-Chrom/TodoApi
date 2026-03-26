using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Query
{
    public record GetTodoById(long Id, string UserId);

    public class GetTodoByIdHandler
    {
        public readonly IReadonlyRepository _repository;
        public GetTodoByIdHandler(IReadonlyRepository repository)
        {
            _repository = repository;
        }
        public async Task<TodoItem> Handle(GetTodoById query)
        {   
            return await _repository.GetTodoById(query.Id, query.UserId);
        }
    }
}
