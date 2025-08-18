using QuizApi.Enums;

namespace QuizApi.Models;

public class AddQuizImageToQuizcardModel
{
    public required QuizcardIdentifier QuizcardId { get; set; }

    public ImageAssignment Assignment { get; set; } = ImageAssignment.NoAssignment;

    public int? Position { get; set; }
    
    public required string Url { get; set; }
    public string? PublicId { get; set; }
}