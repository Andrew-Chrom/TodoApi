using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Repositories.Auth;
using TodoApi.UOF;

namespace TodoApi.Services
{
    public class TokenIssuerService : ITokenIssuerService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public TokenIssuerService(IAccessTokenService accessTokenService, 
            IRefreshTokenService refreshTokenService, 
            UnitOfWork unitOfWork,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticateResponse> IssueTokensAsync(User user, CancellationToken cancellationToken)
        {
            var refreshToken = _refreshTokenService.Generate(user);
            await _refreshTokenRepository.AddAsync(new RefreshToken(user.Id, refreshToken));
            await _unitOfWork.CompleteAsync();

            return new AuthenticateResponse
            {       
                AccessToken = _accessTokenService.Generate(user),
                RefreshToken = refreshToken
            };
        }
    }
}
