using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoLists
{
    public class ReadonlyTodoListRepository : IReadonlyTodoListRepository
    {
        public readonly QueryDbContext _ctx;
        public ReadonlyTodoListRepository(QueryDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<TodoListResponseDto>> GetTodoList(string userId)
        {
            var todoLists = _ctx.TodoLists.Where(x => x.UserId == userId).Select(x => new TodoListResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Created = x.Created,
                Updated = x.Updated,
                OpenItemsCount = x.OpenItemsCount,
                CompletedItemsCount = x.CompletedItemsCount
            });

            return await todoLists.ToListAsync();
        }
        public async Task<TodoListItemResponseDto?>? GetTodoListById(long id, string userId)
        {
            var todoList = await _ctx.TodoLists
                .Include(tl => tl.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (todoList is null)
                return null;
            return new TodoListItemResponseDto
            {
                Id = todoList.Id,
                Title = todoList.Title,
                Created = todoList.Created,
                Updated = todoList.Updated,
                OpenItemsCount = todoList.OpenItemsCount,
                CompletedItemsCount = todoList.CompletedItemsCount,
                Items = todoList.Items.Select(i => new TodoItemResponseDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    IsComplete = i.IsComplete
                }).ToList()
            };
        }
    }
}
