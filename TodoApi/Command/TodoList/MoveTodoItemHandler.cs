using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Repositories.TodoLists;
using TodoApi.UOF;
using Wolverine.Persistence;

namespace TodoApi.Command.TodoList
{
    public class MoveTodoItemHandler
    {
        public record MoveTodoItemCommand(string UserId, long TodoListId, long TodoItemId, long DestinationTodoListId);

        UnitOfWork _unitOfWork;
        public MoveTodoItemHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MoveTodoItemCommand cmd)
        {
            // COUNTERS. Change counters!!!!

            var todoList = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.TodoListId, cmd.UserId);

            if(todoList == null)
                throw new NotFoundException("Todo list not found");
            
            var todoItem = todoList.Items.Find(item => item.Id == cmd.TodoItemId);

            if (todoItem == null)
                throw new NotFoundException("Todo item not found");

            var list = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.TodoListId, cmd.UserId);
            var destinationList = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.TodoListId, cmd.UserId);

            if (todoItem.IsComplete)
            {
                list.CompletedItemsCount -= 1;
                destinationList.CompletedItemsCount += 1;
            }
            else
            {
                list.OpenItemsCount -= 1;
                destinationList.OpenItemsCount += 1;
            }

            await _unitOfWork.TodoItemRepo.UpdateTodo(new TodoItemCreateDTO
            {
                IsComplete = todoItem.IsComplete,
                Name = todoItem.Name,
                TodoListId = null
            }, todoItem.Id, cmd.UserId);

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
