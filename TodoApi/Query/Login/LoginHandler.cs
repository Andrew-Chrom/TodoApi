using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Errors;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Query.Login
{
    public class LoginHandler
    {

        private readonly UserManager<User> _userManager;
        private readonly IAuthenticateService _authService;

        public LoginHandler(UserManager<User> userManager, IAuthenticateService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }
        public async Task<AuthenticateResponse> Handle(LoginQuery query)
        {
            var user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null)
                throw new NotFoundException("User not found");

            if (await _userManager.CheckPasswordAsync(user, query.Password))
                return await _authService.Authenticate(user, default);
            else
                throw new Exception("Unauthorized");
        }

    }
}
