using TodoApi.Models.DTO;
using TodoApi.UOF;

namespace TodoApi.Command.TodoList
{
    public record CreateTodoListCommand(string Title, string UserId);

    public class CreateTodoListHandler
    {
        public readonly UnitOfWork _unitOfWork;
        public CreateTodoListHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<long> Handle(CreateTodoListCommand command)
        {
            var todo = new TodoListCreateDto(command.Title);

            var id = await _unitOfWork.TodoListRepo.CreateTodoList(todo, command.UserId);
            await _unitOfWork.CompleteAsync();

            return id;
        }
    }
}