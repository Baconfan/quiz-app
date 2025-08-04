namespace QuizApi.Models;

public class RegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string DisplayName { get; set; } = "";

    public string[] Roles { get; set; } = [];
}