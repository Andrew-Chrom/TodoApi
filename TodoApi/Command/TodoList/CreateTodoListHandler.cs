using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using NuGet.Protocol.Core.Types;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.Command.TodoList
{
    public record CreateTodoListCommand(string Title, string UserId);

    public class CreateTodoListHandler
    {
        public readonly IWritableTodoListRepository _repository;
        public CreateTodoListHandler(IWritableTodoListRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> Handle(CreateTodoListCommand command)
        {
            var todo = new TodoListCreateDto(command.Title);

            var id = await _repository.CreateTodoList(todo, command.UserId);

            return id;
        }
    }
}