using Microsoft.AspNetCore.Identity;

namespace Point.API.Controllers.Authentication;
public interface IAuthenticationService
{
    Task<AuthenticationResult> RegisterAsync(string username, string password);
    Task<AuthenticationResult> LoginAsync(string username, string password);
    Task LogoutAsync();
}