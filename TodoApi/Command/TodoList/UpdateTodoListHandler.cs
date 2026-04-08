using NuGet.Protocol.Core.Types;
using TodoApi.Models.DTO;
using TodoApi.Repositories;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.Command.TodoList
{

    public record UpdateTodoCommand(long id, string Title, string UserId);
    public class UpdateTodoListHandler
    {
        public readonly IWritableTodoListRepository _repository;
        public UpdateTodoListHandler(IWritableTodoListRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdateTodoCommand cmd)
        {

            var dto = new TodoListCreateDto(cmd.Title);

            await _repository.UpdateTodoList(dto, cmd.id, cmd.UserId);
        }
    }
}

   

