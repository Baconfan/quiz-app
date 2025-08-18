namespace QuizApi.Dtos.Input;

public class UpdateQuizboardValuesDto
{
    public required string QuizboardId { get; set; }

    public required int[] NewValues { get; set; }
}