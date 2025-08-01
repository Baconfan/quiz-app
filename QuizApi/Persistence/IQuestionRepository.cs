using QuizApi.Entities;
using QuizApi.Models;

namespace QuizApi.Persistence;

public interface IQuestionRepository
{
    Task<List<Question>> GetQuestions();
}