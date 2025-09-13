
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;

namespace Point.API.Controllers.Authentication;

[ApiController]
[Route("api/v{version:apiversion}/auth")]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string password)
    {
        var result = await _authenticationService.RegisterAsync(username, password);
        if (result.Result)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var result = await _authenticationService.LoginAsync(username, password);
        if (result.Result)
            return Ok(result);
        return Unauthorized(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authenticationService.LogoutAsync();
        return Ok("Logged out successfully");
    }

}