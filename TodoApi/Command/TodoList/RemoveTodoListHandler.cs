using TodoApi.Models.DTO;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;
using TodoApi.UOF;

namespace TodoApi.Command.TodoList
{
    public record RemoveTodoListCommand(long TodoListId, long TodoItemId, string UserId);
    public class RemoveTodoListHandler
    {
        public readonly IReadonlyTodoItemRepository _todoItemReadonlyRepository;
        public readonly UnitOfWork _unitOfWork;
        public readonly ILogger<RemoveTodoListHandler> _logger;
        public RemoveTodoListHandler(
            IReadonlyTodoItemRepository todoItemReadonlyRepository,
            UnitOfWork unitOfWork,
            ILogger<RemoveTodoListHandler> logger)
        {
            _todoItemReadonlyRepository = todoItemReadonlyRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(RemoveTodoListCommand command)
        {

            var todoItem = await _todoItemReadonlyRepository.GetTodoById(command.TodoItemId, command.UserId);
            _logger.LogInformation("Removing todo item with id {TodoItemId} from list with id {TodoListId}", command.TodoItemId, command.TodoListId);
            
            if(todoItem.TodoListId == null)
            {
                throw new Exception($"Todo item with id {command.TodoItemId} is not associated with any list.");
            }
            await _unitOfWork.TodoItemRepo.UpdateTodo(new TodoItemCreateDTO
            {
                IsComplete = todoItem.IsComplete,
                Name = todoItem.Name,
                TodoListId = null
            }, todoItem.Id, command.UserId);



            var list = await _unitOfWork.TodoListRepo.GetTodoListById(command.TodoListId, command.UserId);

            if (todoItem.IsComplete)
            {
                list.CompletedItemsCount -= 1;
            }
            else
            {
                list.OpenItemsCount -= 1;
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
