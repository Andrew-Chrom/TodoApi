using TodoApi.Models.DTO;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;
using Microsoft.Extensions.Logging;
using TodoApi.UOF;



namespace TodoApi.Command.TodoList
{
    public record AddTodoListCommand(long TodoListId, long TodoItemId, string UserId);
    public class AddTodoListHandler
    {
        public readonly IReadonlyTodoItemRepository _todoItemReadonlyRepository;
        public readonly ILogger<AddTodoListHandler> _logger;
        public readonly UnitOfWork _unitOfWork;
        public AddTodoListHandler(
            IReadonlyTodoItemRepository todoItemReadonlyRepository,
            ILogger<AddTodoListHandler> logger,
            UnitOfWork unitOfWork)
        {

            _todoItemReadonlyRepository = todoItemReadonlyRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(AddTodoListCommand command)
        {

            var todoItem = await _todoItemReadonlyRepository.GetTodoById(command.TodoItemId, command.UserId);

            _logger.LogInformation("Before UpdateTodo: command.TodoListId={TodoListId}", command.TodoListId);

            await _unitOfWork.TodoItemRepo.UpdateTodo(new TodoItemCreateDTO
            {
                IsComplete = todoItem.IsComplete,
                Name = todoItem.Name,
                TodoListId = command.TodoListId
            }, todoItem.Id, command.UserId);

            var list = await _unitOfWork.TodoListRepo.GetTodoListById(command.TodoListId, command.UserId);

            if (todoItem.IsComplete)
            {
                list.CompletedItemsCount += 1;
            }
            else
            {
                list.OpenItemsCount += 1;
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
