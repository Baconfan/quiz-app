using Microsoft.AspNetCore.Mvc;
using QuizApi.InputDto;
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
            ? NotFound($"Kein Board zur ID {id} gefunden")
            : matchingBoard;
    }

    // Update Quizboard
    [HttpPut("update/categories")]
    public async Task<IActionResult> UpdateQuizboardCategories(UpdateQuizboardCategoryDto dto)
    {
        await _quizboardRepository.UpdateQuizboardCategories(dto);

        return NoContent();
    }

    [HttpPut("update/values")]
    public async Task<IActionResult> UpdateQuizboardValues(UpdateQuizboardValuesDto dto)
    {
        await _quizboardRepository.UpdateQuizboardValues(dto);
        
        return NoContent();
    }

    // Create Quizboard By ID

    // Delete Quizboard By ID
    
}