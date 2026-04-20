using Microsoft.AspNetCore.Identity;
using TodoApi.Models;
using TodoApi.Repositories.Auth;

namespace TodoApi.Command.Authorization
{
    public record RegisterCommand(string Email, string Password);
    public class RegisterHandler
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IdentityResult> Handle(RegisterCommand cmd)
        {
            if (await _userRepository.GetUserByEmailAsync(cmd.Email) is not null)
            {
                throw new Exception("User with this email exists");
            }

            var user = new User(cmd.Email)
            {
                UserName = cmd.Email,
                Email = cmd.Email
            };

            return await _userRepository.AddUserAsync(user, cmd.Password);
            
        }

    }
}
