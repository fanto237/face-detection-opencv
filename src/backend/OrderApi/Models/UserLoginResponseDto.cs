namespace OrderApi.Models;

public class UserLoginResponseDto
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = null!;
}