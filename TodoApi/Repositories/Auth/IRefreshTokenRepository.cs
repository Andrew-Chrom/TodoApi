using TodoApi.Models;

namespace TodoApi.Repositories.Auth
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByIdAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task RemoveAsync(RefreshToken refreshToken);
    }
}
