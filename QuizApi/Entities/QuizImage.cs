namespace QuizApi.Entities;

public class QuizImage
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? PublicId { get; set; }
}