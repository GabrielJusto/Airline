namespace Airline.DTO;

public class UserRegisterDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Document { get; set; } = null!;
}