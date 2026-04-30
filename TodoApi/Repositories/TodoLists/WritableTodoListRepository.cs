using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Repositories.TodoLists
{
    public class WritableTodoListRepository : IWritableTodoListRepository
    {
        private readonly CommandDbContext _ctx;
        private readonly ILogger<WritableTodoListRepository> _logger;
        public WritableTodoListRepository(CommandDbContext ctx, ILogger<WritableTodoListRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<TodoList?> GetTodoListById(long? id, string userId)
        {
            var todoList = await _ctx.TodoLists
                .Include(tl => tl.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return todoList;
        }
        public async Task<long> CreateTodoList(TodoListCreateDto dto, string userId)
        {
            var item = new TodoList
            {
                Title = dto.Title,
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
        }
        public async Task DeleteTodoList(TodoList todoList)
        {
            _ctx.TodoLists.Remove(todoList);
        }
    }
}
