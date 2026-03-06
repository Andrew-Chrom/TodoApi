using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Services;

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
    public AuthController(
        UserManager<User> userManager,
        IAuthenticateService authService,
        IApplicationDbContext context,
        IRefreshTokenValidator refreshTokenValidator)
    {
        _userManager = userManager;
        _authService = authService;
        _context = context;
        _refreshTokenValidator = refreshTokenValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var user = new User(model.Email)
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok("User created successfully");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var response = await _authService.Authenticate(user, default);
            return Ok(response);
        }

        return Unauthorized("Invalid email or password");
    }   

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model, CancellationToken cancellationToken)
    {

        var isValid = _refreshTokenValidator.Validate(model.RefreshToken);
        if (!isValid) return Unauthorized("Invalid refresh token");

        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == model.RefreshToken);

        if (refreshToken == null) return Unauthorized("Token not found");

        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if (user == null) return Unauthorized("User not found");

        var response = await _authService.Authenticate(user, default);

        return Ok(response);
    }

}