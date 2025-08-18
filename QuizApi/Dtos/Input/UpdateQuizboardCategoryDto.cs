namespace QuizApi.Dtos.Input;

public record UpdateQuizboardCategoryDto
{
    public required string QuizboardId { get; set; }
    
    public required int CategoryId { get; set; }

    public required string NewCategoryName { get; set; }
}