using QuizApi.Dtos.Input;
using QuizApi.Entities;
using QuizApi.Models;

namespace QuizApi.Persistence;

public interface IQuizboardRepository
{
    // Quizboard
    Task<List<QuizboardDto>> GetAllQuizboards();
    
    Task<QuizboardDto?> GetQuizboardById(string quizboardId);
    
    Task UpsertQuizboardCategory(UpdateQuizboardCategoryDto dto);
    
    Task DeleteQuizboardCategory(string quizboardId, int categoryId);
    
    Task UpdateQuizboardValues(UpdateQuizboardValuesDto dto);
    
    
    // Quizcards
    Task CreateNewQuizcard(GamecardDto dto);

    Task UpsertQuizcard(GamecardDto dto);
    
    Task DeleteQuizcardById(DeleteQuizcardDto dto);
    
    Task AddImageToQuizcard(AddQuizImageToQuizcardModel imageReference);

    Task DeleteImageFromQuizcard();
}