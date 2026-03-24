using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Errors;

namespace TodoApi.Command
{
    public record RefreshCommand(string RefreshToken, CancellationToken cancellationToken);
    public class RefreshHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticateService _authService;
        private readonly SignInManager<User> _signInManager;
        private readonly IApplicationDbContext _context;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        public RefreshHandler(
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
        public async Task<AuthenticateResponse> Handle(RefreshCommand cmd)
        {
            var isValid = _refreshTokenValidator.Validate(cmd.RefreshToken);
            if (!isValid) throw new UnathorizedException("Invalid refresh token");

            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == cmd.RefreshToken);

            if (refreshToken == null) throw new UnathorizedException("Token not found");

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync(cmd.cancellationToken);

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user == null) throw new UnathorizedException("User not found");

            var response = await _authService.Authenticate(user, default);

            return response;
        }
    }
}
