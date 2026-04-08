using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    /// <summary>
    /// Interface for authentication.
    /// </summary>
    public interface ITokenIssuer 
    {
        /// <summary>
        /// Authenticates user.
        /// Takes responsibilities to generate access and refresh token, save refresh token in database
        /// and return instance of <see cref="AuthenticateResponse"/> class. 
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Instance of <see cref="CancellationToken"/>.</param>
        Task<AuthenticateResponse> IssueTokensAsync(User user, CancellationToken cancellationToken);
    }
}
