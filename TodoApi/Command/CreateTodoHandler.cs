using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Command
{
    public record CreateTodoCommand(string Name, bool IsComplete, string UserId);
    public class CreateTodoHandler
    {
        public async Task<long> Handle(CreateTodoCommand cmd, CommandDbContext db)
        {
            var todo = new TodoItem
            {
                Name = cmd.Name,
                IsComplete = cmd.IsComplete,
                UserId = cmd.UserId
            };

            db.TodoItems.Add(todo);
            await db.SaveChangesAsync();

            return todo.Id;
        }
    }
}
