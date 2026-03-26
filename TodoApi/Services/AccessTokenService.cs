using Microsoft.Extensions.Options;
using System.Security.Claims;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly JwtSettings _jwtSettings;

        public AccessTokenService(ITokenGenerator tokenGenerator, IOptions<JwtSettings> jwtOptions) =>
            (_tokenGenerator, _jwtSettings) = (tokenGenerator, jwtOptions.Value);

        public string Generate(User user)
        {
            List<Claim> claims = new()
        {
            new Claim("id", user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };
            return _tokenGenerator.Generate(_jwtSettings.AccessTokenSecret, _jwtSettings.Issuer, _jwtSettings.Audience,
                _jwtSettings.AccessTokenExpirationMinutes, claims);
        }
    }
}
