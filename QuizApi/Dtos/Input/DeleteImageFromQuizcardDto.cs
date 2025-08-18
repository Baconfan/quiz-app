using QuizApi.Enums;
using QuizApi.Models;

namespace QuizApi.Dtos.Input;

public class DeleteImageFromQuizcardDto
{
    public required QuizcardIdentifier QuizcardId { get; set; }

    public ImageAssignment Assignment { get; set; } = ImageAssignment.NoAssignment;

    public int? Position { get; set; }
}