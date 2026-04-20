using TodoApi.Errors;
using TodoApi.Models.DTO;
using TodoApi.UOF;
namespace TodoApi.Command.TodoItems
{
    public record ToggleTodoCommand(long Id, string UserId);
    public class ToggleTodoHandler
    {
        public UnitOfWork _unitOfWork;
        public ToggleTodoHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TodoItemResponseDTO> Handle(
        ToggleTodoCommand command)
        {
            var todo = await _unitOfWork.TodoItemRepo.GetTodoById(command.Id, command.UserId);

            if (todo == null)
                throw new NotFoundException("Todo not found");
            await _unitOfWork.TodoItemRepo.UpdateTodo(new TodoItemCreateDTO
            {
                Name = todo.Name,
                IsComplete = !todo.IsComplete,
                TodoListId = todo.TodoListId
            }, command.Id, command.UserId);
            
            var list = await _unitOfWork.TodoListRepo.GetTodoListById(todo.TodoListId, command.UserId);

            if (list != null)
            {
                if (!todo.IsComplete)
                {
                    list.OpenItemsCount--;
                    list.CompletedItemsCount++;
                }
                else
                {
                    list.OpenItemsCount++;
                    list.CompletedItemsCount--;
                }
            }

            await _unitOfWork.CompleteAsync();

            return todo;
        }
    }
}
