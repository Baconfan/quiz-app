namespace QuizApi.Models;

public class UserDto
{
    public required string UserName { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public required string Token { get; set; }
}