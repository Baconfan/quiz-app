using QuizApi.Enums;
using QuizApi.Models;

namespace QuizApi.Dtos.Input;

public class ImageUploadToGamecardDto
{
    /// <summary>
    /// information to identify the quizcard the image belongs to
    /// </summary>
    public required QuizcardIdentifier QuizcardId { get; set; }

    /// <summary>
    /// To which part of the quizcard does the image belong to? 
    /// </summary>
    public ImageAssignment ImageAssignment { get; set; } = ImageAssignment.NoAssignment;

    /// <summary>
    /// Position in the array of the assigned property (e.g. ImageAssignment 1, ArrayPosition 0 -> First answer)
    /// </summary>
    public int? ArrayPosition { get; set; }
}

