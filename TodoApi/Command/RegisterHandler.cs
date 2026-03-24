using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using TodoApi.Models;

namespace TodoApi.Command
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
            var user = new User(cmd.Email)
            {
                UserName = cmd.Email,
                Email = cmd.Email
            };

            var result = await _userManager.CreateAsync(user, cmd.Password);
            return result;
        }

    }
}
