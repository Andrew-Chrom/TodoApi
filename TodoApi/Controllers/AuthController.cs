using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Command.Refresh;
using TodoApi.Command.Register;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Query.Login;
using TodoApi.Services;
using Wolverine;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]


public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticateService _authService;
    private readonly SignInManager<User> _signInManager;
    private readonly IApplicationDbContext _context; 
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IMessageBus _bus;
    public AuthController(
        UserManager<User> userManager,
        IAuthenticateService authService,
        IApplicationDbContext context,
        IRefreshTokenValidator refreshTokenValidator,
        IMessageBus bus)
    {
        _userManager = userManager;
        _authService = authService;
        _context = context;
        _refreshTokenValidator = refreshTokenValidator;
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