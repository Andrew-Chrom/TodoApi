using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Command
{
    public record DeleteTodo(long Id, string UserId);
    public class DeleteTodoHandler
    {
        public async Task Handle(DeleteTodo cmd,
            CommandDbContext db)
        {
            var todoItem = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == cmd.Id && x.UserId == cmd.UserId);

            if (todoItem == null)
                // Consider using not notFound but another exception, because not found is used for get queries
                throw new NotFoundException("Todo not found");

            db.TodoItems.Remove(todoItem);
            await db.SaveChangesAsync();

        }

    }
}
