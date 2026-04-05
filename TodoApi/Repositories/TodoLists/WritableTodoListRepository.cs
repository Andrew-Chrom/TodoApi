using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Security.Claims;
using TodoApi.Command;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Query;
using Wolverine;

namespace TodoApi.Repositories.TodoLists
{
    public class WritableTodoListRepository : IWritableTodoListRepository
    {
        public readonly CommandDbContext _ctx;
        public readonly ILogger<WritableTodoListRepository> _logger;
        public WritableTodoListRepository(CommandDbContext ctx, ILogger<WritableTodoListRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<TodoList>? GetTodoListById(long? id, string userId)
        {
            var todoList = await _ctx.TodoLists.Include(tl => tl.Items).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return todoList;
        }
        public async Task<long> CreateTodoList(TodoListCreateDto dto, string userId)
        {
            var item = new TodoList
            {
                Title = dto.Title,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                OpenItemsCount = 0,
                CompletedItemsCount = 0,
                UserId = userId
            };
            _ctx.TodoLists.Add(item);
            await _ctx.SaveChangesAsync();
            
            return item.Id;
        }
        public async Task UpdateTodoList(TodoListCreateDto dto, long id, string userId)
        {
            var todoList = await _ctx.TodoLists.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoList == null)
                throw new NotFoundException("Todo not found");

            todoList.Title = dto.Title;
            todoList.Updated = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteTodoList(long id, string userId)
        {
            var todoList = await _ctx.TodoLists.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoList == null)
                throw new NotFoundException("Todo not found");

            var items = await _ctx.TodoItems.Where(x => x.TodoListId == id).ToListAsync();
            _logger.LogInformation("Found {Count} items to delete", items.Count);
            _ctx.TodoItems.RemoveRange(items);

            _ctx.TodoLists.Remove(todoList);
            await _ctx.SaveChangesAsync();
        }
    }
}
