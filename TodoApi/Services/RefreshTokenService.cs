using Microsoft.Extensions.Options;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(ITokenGenerator tokenGenerator, IOptions<JwtSettings> jwtOptions) =>
            (_tokenGenerator, _jwtSettings) = (tokenGenerator, jwtOptions.Value);

        public string Generate(User user) => _tokenGenerator.Generate(_jwtSettings.RefreshTokenSecret,
            _jwtSettings.Issuer, _jwtSettings.Audience,
            _jwtSettings.RefreshTokenExpirationMinutes);
    }
}
