using Humanizer;
using Microsoft.EntityFrameworkCore;
using TodoApi.Command.UpdateTodo;
using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Command.UpdateTodo
{
    public class UpdateTodoHandler
    {
        public async Task Handle(UpdateTodoCommand cmd, ApplicationDbContext db)
        {
            var todoItem = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == cmd.Id && x.UserId == cmd.UserId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            todoItem.Name = cmd.Name;
            todoItem.IsComplete = cmd.IsComplete;

            db.Entry(todoItem).State = EntityState.Modified;

            await db.SaveChangesAsync();
        }
    }
}
