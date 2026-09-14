using Airline.DTO;
using Airline.Models;
using Airline.Services.Implementations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(
    AuthService authService) : ControllerBase
{

    private readonly AuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerData)
    {
        IdentityResult result = await _authService.RegisterUser(registerData);
        return this.Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginData)
    {
        LoginResponseDTO result = await _authService.LoginUser(loginData);
        return this.Ok(result);
    }
}
