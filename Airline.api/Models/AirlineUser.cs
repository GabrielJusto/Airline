
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Airline.Models;

public partial class AirlineUser : IdentityUser<int>
{
    public string Document { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
