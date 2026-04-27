using System.Text.Json.Serialization;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.UOF;
using Wolverine.Persistence;

namespace TodoApi.Command.TodoList
{

    public record UpdateTodoCommand
    {
        public string Title { get; set; }
        [JsonIgnore]
        public long Id { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    };
    public class UpdateTodoListHandler
    {
        public readonly UnitOfWork _unitOfWork;
        public UpdateTodoListHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateTodoCommand cmd)
        {
            
            var dto = new TodoListCreateDto(cmd.Title);
            
            var todoList = await _unitOfWork.TodoListRepo.GetTodoListById(cmd.Id, cmd.UserId);

            if (todoList == null)
                throw new NotFoundException("Todo not found");

            await _unitOfWork.TodoListRepo.UpdateTodoList(todoList, dto);
            await _unitOfWork.CompleteAsync();
        }
    }
}

   

