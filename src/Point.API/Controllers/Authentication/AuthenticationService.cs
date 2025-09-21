
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Point.Infrastructure.Identity.Domain.Entities;

namespace Point.API.Controllers.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<AuthenticationResult> LoginAsync(string username, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

        var authResult = new AuthenticationResult()
        {
            Result = result.Succeeded,
            Message = result.Succeeded ? "Logged in successfully" : "Log in failed",
            Code = result.Succeeded ? "LOGIN_SUCCESS" : "LOGIN_FAILED"
        };

        return authResult;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<AuthenticationResult> RegisterAsync(string username, string password)
    {
        var user = new User { UserName = username };
        var result = await _userManager.CreateAsync(user, password);

        var authResult = new AuthenticationResult() { 
            Result = result.Succeeded,
            Message = result.Succeeded ? "Registration successful" : "Registration has failed",
            Errors = result.Errors!.ToArray(),
            Code = result.Succeeded ? "REGISTER_SUCCESS" : "REGISTER_FAILED"
        };

        return authResult;
    }
}