using Microsoft.AspNetCore.Identity;
using TodoApi.Models;

namespace TodoApi.Repositories.Auth
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> CheckUserPassword(User user, string password);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);

    }
}
