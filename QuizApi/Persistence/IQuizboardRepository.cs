using QuizApi.InputDto;
using QuizApi.Models;

namespace QuizApi.Persistence;

public interface IQuizboardRepository
{
    // Quizboard
    Task<List<QuizboardDto>> GetAllQuizboards();
    
    Task<QuizboardDto?> GetQuizboardById(string quizboardId);
    
    Task UpdateQuizboardCategories(UpdateQuizboardCategoryDto dto);
    
    Task UpdateQuizboardValues(UpdateQuizboardValuesDto dto);
    
    
    // Quizcards
    Task CreateNewQuizcard(GamecardDto dto);

    Task UpdateQuizcard(GamecardDto dto);
    
    Task DeleteQuizcardById(DeleteQuizcardDto dto);
}