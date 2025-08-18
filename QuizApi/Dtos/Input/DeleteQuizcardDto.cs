namespace QuizApi.Dtos.Input;

public record DeleteQuizcardDto
{
    public required string QuizboardId { get; init; }
    
    public required int CategoryId { get; init; }
    
    public required int ValueId { get; init; }
}