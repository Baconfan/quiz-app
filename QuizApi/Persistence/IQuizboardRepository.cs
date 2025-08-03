using QuizApi.InputDto;
using QuizApi.Models;

namespace QuizApi.Persistence;

public interface IQuizboardRepository
{
    Task<List<QuizboardDto>> GetAllQuizboards();
    
    Task<QuizboardDto?> GetQuizboardById(string quizboardId);
    
    Task UpdateQuizboardCategories(UpdateQuizboardCategoryDto dto);
    
    Task UpdateQuizboardValues(UpdateQuizboardValuesDto dto);
}