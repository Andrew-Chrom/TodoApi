using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.UOF;

namespace TodoApi.Command.TodoList
{
    public record DeleteTodoListCommand(long Id, string UserId);
    public class DeleteTodoListHandler
    {

        public readonly UnitOfWork _unitOfWork;
        public DeleteTodoListHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteTodoListCommand cmd)
        {
            
            var todoList = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.Id, cmd.UserId);

            if (todoList == null)
                throw new NotFoundException("Todo not found");

            await _unitOfWork.TodoListRepo.DeleteTodoList(todoList);
            await _unitOfWork.CompleteAsync();
        }

    }
}
