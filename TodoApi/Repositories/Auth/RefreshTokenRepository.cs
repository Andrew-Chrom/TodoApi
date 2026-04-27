using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;

namespace TodoApi.Repositories.Auth
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        public CommandDbContext _db {  get; set; }

        public RefreshTokenRepository(CommandDbContext db)
        {
            _db = db;
        }
        public async Task<RefreshToken> GetByIdAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }
        public async Task AddAsync(RefreshToken refreshToken)
        {
            _db.RefreshTokens.Add(refreshToken);
        }
        public async Task RemoveAsync(RefreshToken refreshToken)
        {
            _db.RefreshTokens.Remove(refreshToken);
        }
    }
}
