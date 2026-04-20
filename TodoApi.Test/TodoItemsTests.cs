using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TodoApi.Command.TodoItems;
using TodoApi.Command.TodoList;
using TodoApi.Controllers;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Query;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;
using TodoApi.UOF;
using Xunit;

namespace TodoApi.TodoApi.Test
{
    public class TodoItemsTests
    {
        private CommandDbContext GetCommandDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            return new CommandDbContext(options);
        }

        private QueryDbContext GetQueryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            return new QueryDbContext(options);
        }

        [Fact]
        public async Task ToggleItem_ChangeState()
        {
            var context = GetCommandDbContext();
            context.TodoItems.Add(new TodoItem { Id = 1L, Name = "Walk Dog", IsComplete = false, UserId = "test-user-id" });

            await context.SaveChangesAsync();

            var logger = NullLogger<WritableTodoListRepository>.Instance;
            var handler = new ToggleTodoHandler(new UnitOfWork(context, logger));

            var userId = "test-user-id";
            await handler.Handle(new ToggleTodoCommand(1L, userId));

            var TodoItem = await context.TodoItems.FindAsync(1L);

            Assert.True(TodoItem.IsComplete);
        }

        [Fact]
        public async Task GetAllProducts_ReturnAllProducts()
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";
            context.TodoItems.Add(new TodoItem { Id = 1, Name = "Walk Dog", IsComplete = false, UserId = userId });
            context.TodoItems.Add(new TodoItem { Id = 2, Name = "Wash dishes", IsComplete = false, UserId = userId });
            await context.SaveChangesAsync();

            var handler = new GetTodosQueryHandler(new ReadonlyTodoItemRepository(context));

            var result = handler.Handle(new GetTodosQuery(userId, null)).Result;
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTodoItems_ReturnsEmptyList_WhenNoItemsExist()
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";

            var handler = new GetTodosQueryHandler(new ReadonlyTodoItemRepository(context));

            var result = handler.Handle(new GetTodosQuery(userId, null)).Result;

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTodoItem_ExistingElement()
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true, UserId=userId });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false, UserId = userId });

            await context.SaveChangesAsync();

            var handler = new GetTodoByIdHandler(new ReadonlyTodoItemRepository(context));
            var result = handler.Handle(new GetTodoById(1L, userId)).Result;

            var targetTodo = await context.TodoItems.FindAsync(1L);

            Assert.Equal(targetTodo.Id, result.Id);
            Assert.Equal(targetTodo.Name, result.Name);
            Assert.Equal(targetTodo.IsComplete, result.IsComplete);
            Assert.Equal(targetTodo.UserId, result.UserId);
        }

        [Theory]
        [InlineData(-1L)]
        [InlineData(999L)]
        public async Task GetTodoItem_NonExistingElement(long id)
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true, UserId = userId });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false, UserId = userId });

            await context.SaveChangesAsync();
            
            var handler = new GetTodoByIdHandler(new ReadonlyTodoItemRepository(context));

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetTodoById(id, userId)));
        }

        [Fact]
        public async Task DeleteTodoItem_ExistingElement()
        {
            var context = GetCommandDbContext();
            
            var userId = "test-user-id";
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true, UserId = userId });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false, UserId = userId });

            await context.SaveChangesAsync();

            var handler = new DeleteTodoHandler(new UnitOfWork(context, NullLogger<WritableTodoListRepository>.Instance));
            var result = handler.Handle(new DeleteTodo(1L, "test-user-id"));
            
            var TodoItems = await context.TodoItems.ToListAsync();

            Assert.Single(TodoItems);
        }


        [Theory]
        [InlineData(-1L)]
        [InlineData(999L)]
        public async Task DeleteTodoItem_NonExistingElement(long id)
        {
            var context = GetCommandDbContext();

            var userId = "test-user-id";
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true, UserId = userId });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false, UserId = userId });

            await context.SaveChangesAsync();

            var handler = new DeleteTodoHandler(new UnitOfWork(context, NullLogger<WritableTodoListRepository>.Instance));

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new DeleteTodo(id, userId)));
        }

    }
}