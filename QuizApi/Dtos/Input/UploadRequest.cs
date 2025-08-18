namespace QuizApi.Dtos.Input;

public class UploadRequest
{
    public IFormFile? File { get; set; }
    public string? MetaData { get; set; }
}