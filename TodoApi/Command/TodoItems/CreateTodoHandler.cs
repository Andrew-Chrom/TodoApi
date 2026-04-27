using System.Text.Json.Serialization;
using TodoApi.Models;
using TodoApi.UOF;

namespace TodoApi.Command.TodoItems
{
    public record CreateTodoCommand
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    };

    public class CreateTodoHandler
    {
        public readonly UnitOfWork _unitOfWork;
        public CreateTodoHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<long> Handle(CreateTodoCommand cmd)
        {
            var todo = new TodoItem
            {
                Name = cmd.Name,
                IsComplete = cmd.IsComplete,
                UserId = cmd.UserId
            };

            var id = await _unitOfWork.TodoItemRepo.CreateTodo(todo);
            await _unitOfWork.CompleteAsync();
            return id;
        }
    }
}
