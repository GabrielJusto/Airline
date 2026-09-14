namespace Airline.DTO;

public class LoginResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
