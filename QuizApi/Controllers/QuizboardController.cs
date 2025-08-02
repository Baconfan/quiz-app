using Microsoft.AspNetCore.Mvc;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class QuizboardController: BaseApiController
{
    private readonly IQuizboardRepository _quizboardRepository;

    public QuizboardController(IQuizboardRepository quizboardRepository)
    {
        _quizboardRepository = quizboardRepository;
    }

    [HttpGet("all")]
    public async Task<List<QuizboardDto>> GetAllQuizboards() 
        =>  await _quizboardRepository.GetAllQuizboards();


    // Get Quizboard By Id
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizboardDto>> GetQuizboardById(string id)
    {
        var matchingBoard =  await _quizboardRepository.GetQuizboardById(id);
        
        return matchingBoard is null
            ? NotFound()
            : matchingBoard;
    }

    // Update Quizboard By ID


    // Create Quizboard By ID

    // Delete Quizboard By ID
    
    // Insert Gamecard
    
    // Update Gamecard
    
    // Delete Gamecard
}