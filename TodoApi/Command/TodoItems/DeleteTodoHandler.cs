using TodoApi.UOF;

namespace TodoApi.Command.TodoItems
{
    public record DeleteTodo(long Id, string UserId);
    public class DeleteTodoHandler
    {

        public readonly UnitOfWork _unitOfWork;
        public DeleteTodoHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteTodo cmd)
        {
            await _unitOfWork.TodoItemRepo.DeleteTodo(cmd.Id, cmd.UserId);
            await _unitOfWork.CompleteAsync();
        }

    }
}
