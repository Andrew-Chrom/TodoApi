using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Repositories;
namespace TodoApi.Command
{
    public record CreateTodoCommand(string Name, bool IsComplete, string UserId);
    public class CreateTodoHandler
    {
        public readonly IWritableRepository _repository;
        public CreateTodoHandler(IWritableRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> Handle(CreateTodoCommand cmd)
        {
            var todo = new TodoItem
            {
                Name = cmd.Name,
                IsComplete = cmd.IsComplete,
                UserId = cmd.UserId
            };

            var id = await _repository.CreateTodo(todo);

            return id;
        }
    }
}
