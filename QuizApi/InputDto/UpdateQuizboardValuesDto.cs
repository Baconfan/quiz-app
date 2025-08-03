namespace QuizApi.InputDto;

public class UpdateQuizboardValuesDto
{
    public required string QuizboardId { get; set; }

    public required int[] NewValues { get; set; }
}