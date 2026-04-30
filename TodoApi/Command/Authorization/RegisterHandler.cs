using Microsoft.AspNetCore.Identity;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Repositories.Auth;

namespace TodoApi.Command.Authorization
{
    public record RegisterCommand(string Email, string Password);
    public class RegisterHandler
    {
        private readonly UserManager<User> _userManager;

        public RegisterHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> Handle(RegisterCommand cmd)
        {
            if (await _userManager.FindByEmailAsync(cmd.Email) != null)
            {
                throw new ConflictException("Email already in use.");
            }

            var user = new User(cmd.Email)
            {
                UserName = cmd.Email,
                Email = cmd.Email
            };

            return await _userManager.CreateAsync(user, cmd.Password);  
        }

    }
}
