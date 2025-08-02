using QuizApi.Models;

namespace QuizApi.Persistence;

public interface IQuizboardRepository
{
    Task<List<QuizboardDto>> GetAllQuizboards();
    
    Task<QuizboardDto?> GetQuizboardById(string quizboardId);
    
    Task<QuizboardDto> UpdateQuizboard(QuizboardDto quizboard);
}