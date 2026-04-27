using System.Text.Json.Serialization;
using TodoApi.Errors;
using TodoApi.Models.DTO;
using TodoApi.UOF;

namespace TodoApi.Command.TodoList
{
    public class MoveTodoItemHandler
    {
        public record MoveTodoItemCommand
        {
            [JsonIgnore]
            public string? UserId { get; set; }
            [JsonIgnore]
            public long TodoListId { get; set; }
            [JsonIgnore]
            public long DestinationTodoListId { get; set; }
            public long TodoItemId { get; set; }

        };

        private readonly UnitOfWork _unitOfWork;
        public MoveTodoItemHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MoveTodoItemCommand cmd)
        {
            var todoList = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.TodoListId, cmd.UserId);

            if(todoList == null)
                throw new NotFoundException("TodoList not found");
            
            var todoItem = todoList.Items.FirstOrDefault(item => item.Id == cmd.TodoItemId);

            if (todoItem == null)
                throw new NotFoundException("TodoItem not found");

            var destinationList = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.DestinationTodoListId, cmd.UserId);

            if (destinationList == null)
                throw new NotFoundException("Destination TodoList not found");

            if (todoItem.IsComplete)
            {
                todoList.CompletedItemsCount -= 1;
                destinationList.CompletedItemsCount += 1;
            }
            else
            {
                todoList.OpenItemsCount -= 1;
                destinationList.OpenItemsCount += 1;
            }
            
            await _unitOfWork.TodoItemRepo.UpdateTodo(new TodoItemCreateDTO
            {
                IsComplete = todoItem.IsComplete,
                Name = todoItem.Name,
                TodoListId = cmd.DestinationTodoListId
            }, todoItem.Id, cmd.UserId);



            await _unitOfWork.CompleteAsync();
        }
        
    }
}
