using TodoApi.Models;
using TodoApi.UOF;

namespace TodoApi.Command.TodoItems
{
    public record CreateTodoCommand(string Name, bool IsComplete, string UserId);
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
