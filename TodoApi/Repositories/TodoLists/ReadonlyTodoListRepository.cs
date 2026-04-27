using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoLists
{
    public class ReadonlyTodoListRepository : IReadonlyTodoListRepository
    {
        private readonly QueryDbContext _ctx;
        public ReadonlyTodoListRepository(QueryDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<TodoListResponseDto>> GetTodoList(string userId)
        {
            var todoLists = _ctx.TodoLists
                .Where(x => x.UserId == userId)
                .Select(x => new TodoListResponseDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Created = x.Created,
                    Updated = x.LastModified,
                    OpenItemsCount = x.OpenItemsCount,
                    CompletedItemsCount = x.CompletedItemsCount
                });

            return await todoLists.ToListAsync();
        }
        public async Task<TodoListItemResponseDto?>? GetTodoListById(long id, string userId)
        {
            var todoList = await _ctx.TodoLists
                .Where(tl => tl.Id == id && tl.UserId == userId)
                .Select(tl => new TodoListItemResponseDto
                {
                    Id = tl.Id,
                    Title = tl.Title,
                    Created = tl.Created,
                    Updated = tl.LastModified,
                    OpenItemsCount = tl.OpenItemsCount,
                    CompletedItemsCount = tl.CompletedItemsCount,
                    Items = tl.Items.Select(i => new TodoItemResponseDTO
                    {
                        Id = i.Id,
                        Name = i.Name,
                        IsComplete = i.IsComplete
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return todoList; 
        }
    }
}
