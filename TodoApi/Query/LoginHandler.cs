using Microsoft.AspNetCore.Identity;
using TodoApi.Errors;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Repositories.Auth;

namespace TodoApi.Query
{
    public record LoginQuery(string Email, string Password);

    public class LoginHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenIssuerService _authService;

        public LoginHandler(UserManager<User> userManager, ITokenIssuerService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }
        public async Task<AuthenticateResponse> Handle(LoginQuery query)
        {
            var user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null)
                throw new UnathorizedException("");

            if (await _userManager.CheckPasswordAsync(user, query.Password))
                return await _authService.IssueTokensAsync(user, default);
            else
                throw new UnathorizedException("");
        }

    }
}
