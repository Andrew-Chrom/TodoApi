using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using TodoApi.Command.TodoList;
using TodoApi.Controllers;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Query;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;
using TodoApi.UOF;
using Xunit;
using static TodoApi.Command.TodoList.MoveTodoItemHandler;

namespace TodoApi.TodoApi.Test
{
    public class TodoListTests
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

        private (CommandDbContext, QueryDbContext) GetContexts()
        {
            var dbName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return (new CommandDbContext(options), new QueryDbContext(options));
        }

        [Fact]
        public async Task GetAllTodoLists_ReturnAllProducts()
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";
            context.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });
            context.Add(new TodoList
            {
                Id = 2,
                Title = "Home",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            }); await context.SaveChangesAsync();

            var handler = new GetTodoListQueryHandler(new ReadonlyTodoListRepository(context));
            var result = handler.Handle(new GetTodoListQuery(userId)).Result;

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTodoLists_ReturnsEmptyList_WhenNoItemsExist()
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";

            var handler = new GetTodoListQueryHandler(new ReadonlyTodoListRepository(context));
            var result = handler.Handle(new GetTodoListQuery(userId)).Result;

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTodoList_ExistingElement()
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";
            context.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });
            context.Add(new TodoList
            {
                Id = 2,
                Title = "Home",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });

            await context.SaveChangesAsync();

            var handler = new GetTodoListByIdHandler(new ReadonlyTodoListRepository(context));

            var result = handler.Handle(new GetTodoListByIdQuery(1, userId)).Result;

            var targetTodoList = await context.TodoLists.FindAsync(1L);

            Assert.Equal(targetTodoList.Id, result.Id);
            Assert.Equal(targetTodoList.Title, result.Title);
            Assert.Equal(targetTodoList.Created, result.Created);
            Assert.Equal(targetTodoList.Updated, result.Updated);
            Assert.Equal(targetTodoList.Updated, result.Updated);
            Assert.Equal(targetTodoList.CompletedItemsCount, result.CompletedItemsCount);
            Assert.Equal(targetTodoList.OpenItemsCount, result.OpenItemsCount);
        }

        [Theory]
        [InlineData(-1L)]
        [InlineData(999L)]
        public async Task GetTodoList_NonExistingElement(long id)
        {
            var context = GetQueryDbContext();
            var userId = "test-user-id";
            context.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });
            context.Add(new TodoList
            {
                Id = 2,
                Title = "Home",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });

            await context.SaveChangesAsync();

            var handler = new GetTodoListByIdHandler(new ReadonlyTodoListRepository(context));

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetTodoListByIdQuery(id, userId)));
        }

        [Fact]
        public async Task DeleteTodoList_ExistingElement()
        {
            var context = GetCommandDbContext();
            var userId = "test-user-id";
            context.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });
            context.Add(new TodoList
            {
                Id = 2,
                Title = "Home",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });

            await context.SaveChangesAsync();

            var handler = new DeleteTodoListHandler(new WritableTodoListRepository(context, NullLogger<WritableTodoListRepository>.Instance));
            await handler.Handle(new DeleteTodoListCommand(1, userId));

            var deletedTodoList = await context.TodoLists.FindAsync(1L);
            Assert.Null(deletedTodoList);
        }
    


        [Fact]
        public async Task AddTodoItemToTodoList_ExistingElement()
        {
            var (cmd_ctx, qr_ctx) = GetContexts();

            var userId = "test-user-id";


            cmd_ctx.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });

            cmd_ctx.TodoItems.Add(new TodoItem { Id = 1L, Name = "Read a book", IsComplete = false, UserId = userId });

            await cmd_ctx.SaveChangesAsync();

            var handler = new AddTodoListHandler(new ReadonlyTodoItemRepository(qr_ctx),
                NullLogger<AddTodoListHandler>.Instance,
                new UnitOfWork(cmd_ctx, NullLogger<WritableTodoListRepository>.Instance));

            await handler.Handle(new AddTodoListCommand(1, 1, userId));
            var todoList = await cmd_ctx.TodoLists.Include(tl => tl.Items).FirstOrDefaultAsync(tl => tl.Id == 1L);

            Assert.NotNull(todoList);
            Assert.Single(todoList.Items);
            Assert.Equal("Read a book", todoList.Items.First().Name);
        }

        [Fact]
        public async Task RemoveTodoItemToTodoList_ExistingElement()
        {
            var (cmd_ctx, qr_ctx) = GetContexts();
            var userId = "test-user-id";
            cmd_ctx.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });

            cmd_ctx.TodoItems.Add(new TodoItem { Id = 1L, Name = "Read a book", 
                IsComplete = false, 
                UserId = userId, 
                TodoListId = 1 });

            await cmd_ctx.SaveChangesAsync();

            var handler = new RemoveTodoListHandler(new ReadonlyTodoItemRepository(qr_ctx),
                new UnitOfWork(cmd_ctx, NullLogger<WritableTodoListRepository>.Instance),
                NullLogger<RemoveTodoListHandler>.Instance);

            await handler.Handle(new RemoveTodoListCommand(1, 1, userId));
            var todoList = await cmd_ctx.TodoLists.Include(tl => tl.Items).FirstOrDefaultAsync(tl => tl.Id == 1L);

            Assert.NotNull(todoList);
            Assert.Empty(todoList.Items);
        }

        [Fact]
        public async Task MoveItemBetweenTwoTodoList()
        {
            var (cmd_ctx, qr_ctx) = GetContexts();
            var userId = "test-user-id";
            cmd_ctx.Add(new TodoList
            {
                Id = 1,
                Title = "Studying",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });
            cmd_ctx.Add(new TodoList
            {
                Id = 2,
                Title = "Home",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = userId
            });

            cmd_ctx.TodoItems.Add(new TodoItem
            {
                Id = 1L,
                Name = "Read a book",
                IsComplete = false,
                UserId = userId,
                TodoListId = 1
            });

            await cmd_ctx.SaveChangesAsync();

            var handler = new MoveTodoItemHandler(new UnitOfWork(cmd_ctx, NullLogger<WritableTodoListRepository>.Instance));
            await handler.Handle(new MoveTodoItemCommand(userId, 1, 1, 2));

            var todoList = await cmd_ctx.TodoLists.Include(tl => tl.Items).FirstOrDefaultAsync(tl => tl.Id == 2L);

            Assert.NotNull(todoList);
            Assert.Single(todoList.Items);
            Assert.Equal("Read a book", todoList.Items.First().Name);
        }

    }
}