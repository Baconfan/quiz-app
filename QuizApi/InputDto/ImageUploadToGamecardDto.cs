using QuizApi.Models;

namespace QuizApi.InputDto;

public class ImageUploadToGamecardDto
{
    public required IFormFile ToBeUploadedFile { get; set; }

    /// <summary>
    /// information to identify the quizcard the image belongs to
    /// </summary>
    public required QuizcardIdentifier QuizcardId { get; set; }

    /// <summary>
    /// To which part of the quizcard does the image belong to? 
    /// </summary>
    public ImageUploadAssignment ImageAssigment { get; set; }

}

public enum ImageUploadAssignment
{
    NoAssigment = 0,
    QuestionImage = 1,
    CorrectAnswer = 2,
    WrongAnswer = 3
}