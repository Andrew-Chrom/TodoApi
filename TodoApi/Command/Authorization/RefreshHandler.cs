using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Errors;
using TodoApi.Repositories.Auth;

namespace TodoApi.Command.Authorization
{
    public record RefreshCommand(string RefreshToken, CancellationToken cancellationToken);
    public class RefreshHandler
    {
        private readonly ITokenIssuerService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        public RefreshHandler(
        ITokenIssuerService authService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenValidator refreshTokenValidator)
        {
            _authService = authService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenValidator = refreshTokenValidator;
        }
        public async Task<AuthenticateResponse> Handle(RefreshCommand cmd)
        {
            var isValid = _refreshTokenValidator.Validate(cmd.RefreshToken);
            if (!isValid) throw new UnathorizedException("");

            var refreshToken = await _refreshTokenRepository.GetByIdAsync(cmd.RefreshToken); 
            if (refreshToken == null) throw new UnathorizedException("");

            await _refreshTokenRepository.RemoveAsync(refreshToken);

            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);
            if (user == null) throw new UnathorizedException("");

            var response = await _authService.IssueTokensAsync(user, default);

            return response;
        }
    }
}
