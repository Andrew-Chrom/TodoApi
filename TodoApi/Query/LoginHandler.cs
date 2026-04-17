using TodoApi.Errors;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Repositories.Auth;

namespace TodoApi.Query
{
    public record LoginQuery(string Email, string Password);

    public class LoginHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenIssuerService _authService;

        public LoginHandler(IUserRepository userRepository, ITokenIssuerService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<AuthenticateResponse> Handle(LoginQuery query)
        {
            var user = await _userRepository.GetUserByEmailAsync(query.Email);

            if (user == null)
                throw new UnathorizedException("");

            if (await _userRepository.CheckUserPassword(user, query.Password))
                return await _authService.IssueTokensAsync(user, default);
            else
                throw new UnathorizedException("");
        }

    }
}
