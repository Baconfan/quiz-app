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
    
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizboardDto>> GetQuizboardById(string id)
    {
        var matchingBoard =  await _quizboardRepository.GetQuizboardById(id);
        
        return matchingBoard is null
            ? NotFound($"Kein Board zur ID {id} gefunden")
            : Ok(matchingBoard);
    }
    
    [HttpPut("category/upsert")]
    public async Task<IActionResult> UpsertQuizboardCategory(UpdateQuizboardCategoryDto dto)
    {
        await _quizboardRepository.UpsertQuizboardCategory(dto);

        return NoContent();
    }

    [HttpDelete("{quizboardId}/category/{categoryId:int}")]
    public async Task<IActionResult> DeleteQuizboardCategory(string quizboardId, int categoryId)
    {
        return Ok();
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