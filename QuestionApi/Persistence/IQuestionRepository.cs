using QuestionApi.Models;

namespace QuestionApi.Persistence;

public interface IQuestionRepository
{
    Task<List<Question>> GetQuestions();
}