using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.TodoApi.Test
{
    public class TodoControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task ToggleItem_ChangeState()
        {
            var context = GetDbContext();
            context.TodoItems.Add(new TodoItem { Id = 1, Name = "Walk Dog", IsComplete = false });

            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            await controller.ToggleItem(id: 1L);

            var TodoItem = await context.TodoItems.FindAsync(1L);

            Assert.True(TodoItem.IsComplete);
        }

        [Fact]
        public async Task GetAllProducts_ReturnAllProducts()
        {
            var context = GetDbContext();
            context.TodoItems.Add(new TodoItem { Id = 1, Name = "Walk Dog", IsComplete = false });
            context.TodoItems.Add(new TodoItem { Id = 2, Name = "Wash dishes", IsComplete = false });
            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.GetTodoItems(IsComplete: null);

            var result = actionResult.Value;
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTodoItems_ReturnsEmptyList_WhenNoItemsExist()
        {
            var context = GetDbContext();
            var controller = new TodoItemsController(context);

            var actionResult = await controller.GetTodoItems(null);

            Assert.NotNull(actionResult.Value);
            Assert.Empty(actionResult.Value);
        }

        [Fact]
        public async Task GetTodoItem_ExistingElement()
        {
            var context = GetDbContext();
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false });

            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.GetTodoItem(id: 1);

            var targetTodo = await context.TodoItems.FindAsync(1L);

            Assert.Equal(targetTodo, actionResult.Value);

        }

        [Theory]
        [InlineData(-1L)]
        [InlineData(999L)]
        public async Task GetTodoItem_NonExistingElement(long id)
        {
            var context = GetDbContext();
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false });

            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.GetTodoItem(id: id);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task DeleteTodoItem_ExistingElement()
        {
            var context = GetDbContext();

            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false });

            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.DeleteTodoItem(id: 1L);

            //Assert.IsType<NotFoundResult>(actionResult.Result);
            var TodoItems = await context.TodoItems.ToListAsync();
            Assert.Equal(1, TodoItems.Count());
        }


        [Theory]
        [InlineData(-1L)]
        [InlineData(999L)]
        public async Task DeleteTodoItem_NonExistingElement(long id)
        {
            var context = GetDbContext();
            context.Add(new TodoItem { Id = 1, Name = "Wash dishes", IsComplete = true });
            context.Add(new TodoItem { Id = 2, Name = "Walk dog", IsComplete = false });

            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.DeleteTodoItem(id: id);

            Assert.IsType<NotFoundResult>(actionResult);
        }

    }
}