using Microsoft.EntityFrameworkCore;
using TodoApi.Command.DeleteTodo;
using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Command.DeleteTodo
{
    public class DeleteTodoHandler
    {
        public async Task Handle(DeleteTodo cmd,
            ApplicationDbContext db)
        {
            var todoItem = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == cmd.Id && x.UserId == cmd.UserId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            db.TodoItems.Remove(todoItem);
            await db.SaveChangesAsync();

        }

    }
}
