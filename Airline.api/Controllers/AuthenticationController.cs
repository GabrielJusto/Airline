

using System.ComponentModel.DataAnnotations;

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

        try
        {
            IdentityResult result = await _authService.RegisterUser(registerData);
            return this.Ok(result);

        }catch(ValidationException e)
        {
            return this.BadRequest(e.Message);
        }
        catch(Exception)
        {
            return this.StatusCode(500, "Internal server error");
        }

    }
}