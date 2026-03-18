using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Command.ToggleTodo
{
    public class ToggleTodoHandler
    {
        public async Task<TodoItem> Handle(
        ToggleTodoCommand command,
        ApplicationDbContext db)
        {
            var todo = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == command.Id && x.UserId == command.UserId);

            if (todo == null)
                throw new NotFoundException("Todo not found");

            todo.IsComplete = !todo.IsComplete;

            await db.SaveChangesAsync();

            return todo;
        }
    }
}
