using QuizApi.Enums;

namespace QuizApi.Models;

public class ImageUploadForFacade
{
    public required Stream ToBeUploadedFile { get; set; }

    public string FileName { get; set; } = "";
    
    public required QuizcardIdentifier QuizcardId { get; set; }

    /// <summary>
    /// To which part of the quizcard does the image belong to? 
    /// </summary>
    public ImageUploadAssignment ImageAssigment { get; set; } = ImageUploadAssignment.NoAssigment;
}