using Humanizer;
using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories;

namespace TodoApi.Command
{
    public record UpdateTodoCommand(long Id, string UserId, string Name, bool IsComplete);
    public class UpdateTodoHandler
    {
        public readonly IWritableRepository _repository;
        public UpdateTodoHandler(IWritableRepository repository)
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
