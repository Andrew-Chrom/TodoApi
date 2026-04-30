using System.Text.Json.Serialization;
using TodoApi.Models.DTO;
using TodoApi.UOF;

namespace TodoApi.Command.TodoItems
{
    public record UpdateTodoCommand 
    {
        [JsonIgnore]
        public  long Id { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
        public string Name {  get; set; }
        public bool IsComplete { get; set; }
    };
    public class UpdateTodoHandler
    {
        public readonly UnitOfWork _unitOfWork;
        public UpdateTodoHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateTodoCommand cmd)
        {

            var dto = new TodoItemCreateDTO
            {
                Name = cmd.Name,
                IsComplete = cmd.IsComplete
            };

            await _unitOfWork.TodoItemRepo.UpdateTodo(dto, cmd.Id, cmd.UserId);
            await _unitOfWork.CompleteAsync();
        }
    }
}
