using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Command;
using TodoApi.Command.Authorization;
using TodoApi.Models;
using TodoApi.Query;
using Wolverine;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]


public class AuthController : Controller
{
    private readonly IMessageBus _bus;
    public AuthController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {

        var result = await _bus.InvokeAsync<IdentityResult>(new RegisterCommand(model.Email, model.Password));

        if (result.Succeeded)
        {
            return Ok("User created successfully");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var result = await _bus.InvokeAsync<AuthenticateResponse>(new LoginQuery(model.Email, model.Password));

        return Ok(result);
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model, CancellationToken cancellationToken)
    {
        var result = await _bus.InvokeAsync<AuthenticateResponse>(new RefreshCommand(model.RefreshToken, cancellationToken));
        return Ok(result);
    }
}