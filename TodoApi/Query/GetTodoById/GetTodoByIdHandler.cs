using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;

namespace TodoApi.Query.GetTodoById
{
    public class GetTodoByIdHandler
    {

        public async Task<TodoItem> Handle(GetTodoById query, ApplicationDbContext db)
        {   
            var todoItem = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == query.Id && x.UserId == query.UserId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            return todoItem;
        }
    }
}
