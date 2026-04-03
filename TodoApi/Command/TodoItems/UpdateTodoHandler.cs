using Humanizer;
using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories.TodoItems;

namespace TodoApi.Command.TodoItems
{
    public record UpdateTodoCommand(long Id, string UserId, string Name, bool IsComplete);
    public class UpdateTodoHandler
    {
        public readonly IWritableTodoItemRepository _repository;
        public UpdateTodoHandler(IWritableTodoItemRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdateTodoCommand cmd)
        {

            var dto = new TodoItemCreateDTO
            {
                Name = cmd.Name,
                IsComplete = cmd.IsComplete
            };

            await _repository.UpdateTodo(dto, cmd.Id, cmd.UserId);
        }
    }
}
