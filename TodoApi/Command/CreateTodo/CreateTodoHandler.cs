using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Command.CreateTodo
{
    public class CreateTodoHandler
    {
        public async Task<long> Handle(CreateTodoCommand cmd, ApplicationDbContext db)
        {
            var todo = new TodoItem
            {
                Name = cmd.Name,
                IsComplete = cmd.IsComplete,
                UserId = cmd.UserId
            };

            if (todo == null)
                throw new NotFoundException("Todo not found");

            db.TodoItems.Add(todo);
            await db.SaveChangesAsync();

            return todo.Id;
        }
    }
}
