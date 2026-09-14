using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Airline.Configuration;
using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Airline.Services.Implementations;

public class AuthService(
    UserManager<AirlineUser> userManager,
    IOptions<JwtConfiguration> jwtOptions
)
{
    private readonly UserManager<AirlineUser> _userManager = userManager;
    private readonly JwtConfiguration _jwtConfiguration = jwtOptions.Value;


    public async Task<IdentityResult> RegisterUser(UserRegisterDTO registerData)
    {

        AirlineUser? userExists = await _userManager.FindByEmailAsync(registerData.Email);
        if(userExists != null)
            throw new ValidationException("Email already registered!");

        AirlineUser user = new()
        {
            Email = registerData.Email,
            UserName = registerData.Email,
            Name = registerData.Name,
            LastName = registerData.LastName,
            Document = registerData.Document
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerData.Password);

        if(!result.Succeeded)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return result;
    }

    public async Task<LoginResponseDTO> LoginUser(UserLoginDTO loginData)
    {
        AirlineUser? user = await _userManager.FindByEmailAsync(loginData.Email);

        if(user is null || !await _userManager.CheckPasswordAsync(user, loginData.Password))
            throw new InvalidCredentialsException(loginData.Email);

        return await BuildToken(user);
    }

    private async Task<LoginResponseDTO> BuildToken(AirlineUser user)
    {
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpiryInMinutes);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, $"{user.Name} {user.LastName}".Trim()),
        ];

        IList<string> roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));

        JwtSecurityToken token = new(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new LoginResponseDTO
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
}
