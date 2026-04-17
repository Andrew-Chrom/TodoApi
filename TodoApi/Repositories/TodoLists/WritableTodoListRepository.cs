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
            
            return item.Id;
        }
        public async Task UpdateTodoList(TodoList todoList, TodoListCreateDto dto)
        {
            todoList.Title = dto.Title;
            todoList.Updated = DateTime.UtcNow;
        }
        public async Task DeleteTodoList(TodoList todoList)
        {
            _ctx.TodoLists.Remove(todoList);
        }
    }
}
