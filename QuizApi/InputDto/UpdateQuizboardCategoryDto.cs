namespace QuizApi.InputDto;

public class UpdateQuizboardCategoryDto
{
    public required string QuizboardId { get; set; }

    public required string[] NewCategories { get; set; }
}