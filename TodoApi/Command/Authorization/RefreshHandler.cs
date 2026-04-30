using Microsoft.AspNetCore.Identity;
using TodoApi.Errors;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Repositories.Auth;
using TodoApi.UOF;

namespace TodoApi.Command.Authorization
{
    public record RefreshCommand(string RefreshToken, CancellationToken cancellationToken);
    public class RefreshHandler
    {
        private readonly ITokenIssuerService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly UnitOfWork _unitOfWork;
        public RefreshHandler(
        ITokenIssuerService authService,
        UserManager<User> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenValidator refreshTokenValidator,
        UnitOfWork unitOfWork)
        {
            _authService = authService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenValidator = refreshTokenValidator;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthenticateResponse> Handle(RefreshCommand cmd)
        {
            var isValid = _refreshTokenValidator.Validate(cmd.RefreshToken);
            if (!isValid) throw new UnathorizedException("");

            var refreshToken = await _refreshTokenRepository.GetByIdAsync(cmd.RefreshToken); 
            if (refreshToken == null) throw new UnathorizedException("");

            await _refreshTokenRepository.RemoveAsync(refreshToken);
            await _unitOfWork.CompleteAsync();
            
            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
            if (user == null) throw new UnathorizedException("");

            var response = await _authService.IssueTokensAsync(user, default);

            return response;
        }
    }
}
