using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.Command.TodoList;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Query;
using Wolverine;
using static TodoApi.Command.TodoList.MoveTodoItemHandler;
namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        public IMessageBus _bus;

        public TodoListController(IMessageBus bus)
        {
            _bus = bus;
        }
        [Authorize]
        [HttpGet]
        public async Task<List<TodoListResponseDto>> GetTodoList()
        {
            var userId = User.FindFirstValue("id");
            return await _bus.InvokeAsync<List<TodoListResponseDto>>(new GetTodoListQuery(userId));
        }

        [Authorize]
        [HttpGet]
        [Route("{todoListId}")]
        public async Task<TodoListItemResponseDto> GetTodoListById(long todoListId)
        {
            var userId = User.FindFirstValue("id");
            return await _bus.InvokeAsync<TodoListItemResponseDto>(new GetTodoListByIdQuery(todoListId, userId));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTodoList(TodoListCreateDto dto)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync<long>(new CreateTodoListCommand(dto.Title, userId));
            
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        [Route("{todoListId}")]
        public async Task<IActionResult> UpdateTodoList(long todoListId, TodoListCreateDto dto)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new UpdateTodoCommand(todoListId, dto.Title, userId));

            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        [Route("{todoListId}")]
        public async Task<IActionResult> DeleteTodoList(long todoListId)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new DeleteTodoListCommand(todoListId, userId));

            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        [Route("remove/{todoListId}/{todoItemId}")]
        public async Task<IActionResult> RemoveTodoListItem(long todoListId, long todoItemId)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new RemoveTodoListCommand(todoListId, todoItemId, userId));

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("add/{todoListId}/{todoItemId}")]
        public async Task<IActionResult> AddTodoListItem(long todoListId, long todoItemId)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new AddTodoListCommand(todoListId, todoItemId, userId));

            return NoContent();
        }

        // Not sure how to name path because move breaks REST because it contains a verb
        [Authorize]
        [HttpPost]
        [Route("{todoListId}/move/{todoItemId}/to/{destinationTodoListId}")]
        public async Task<IActionResult> MoveTodoListItem(long todoListId, long todoItemId, long destinationTodoListId)
        {
            var userId = User.FindFirstValue("id");

            await _bus.InvokeAsync(new MoveTodoItemCommand(userId, todoListId, todoItemId, destinationTodoListId));
               
            return NoContent();
        }

    }
}
