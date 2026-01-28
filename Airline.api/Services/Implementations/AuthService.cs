using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Airline.DTO;
using Airline.Models;

using Microsoft.AspNetCore.Identity;

namespace Airline.Services.Implementations;

public class AuthService(
    UserManager<AirlineUser> userManager
)
{
    private readonly UserManager<AirlineUser> _userManager = userManager;


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
}