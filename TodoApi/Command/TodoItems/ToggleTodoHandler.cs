using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.UOF;
namespace TodoApi.Command.TodoItems
{
    public record ToggleTodoCommand(long Id, string UserId);
    public class ToggleTodoHandler
    {
        public UnitOfWork _unitOfWork;
        public CommandDbContext _ctx;
        public ToggleTodoHandler(CommandDbContext ctx, UnitOfWork unitOfWork)
        {
            _ctx = ctx;
            _unitOfWork = unitOfWork;
        }
        public async Task<TodoItemResponseDTO> Handle(
        ToggleTodoCommand command)
        {
            //var todo = await _ctx.TodoItems.FirstOrDefaultAsync(x => x.Id == command.Id && x.UserId == command.UserId);

            //if (todo == null)
            //    throw new NotFoundException("Todo not found");

            //todo.IsComplete = !todo.IsComplete;

            //if (todo.IsComplete)


            //await _ctx.SaveChangesAsync();

            //return todo;

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
