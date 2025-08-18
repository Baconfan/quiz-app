using QuizApi.Models;

namespace QuizApi.Entities;

public class QuizImage
{
    public required string Url { get; set; }
    public string? PublicId { get; set; }
}