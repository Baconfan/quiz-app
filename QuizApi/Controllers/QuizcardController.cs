using Microsoft.AspNetCore.Mvc;
using QuizApi.InputDto;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class QuizcardController(IQuizboardRepository quizboardRepository) : BaseApiController
{
    [HttpPost("new")]
    public async Task<IActionResult> InsertNewQuizcard([FromBody]GamecardDto dto)
    {
        await quizboardRepository.CreateNewQuizcard(dto);

        return NoContent();
    }
    
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateQuizcard([FromBody] GamecardDto dto)
    {
        await quizboardRepository.UpdateQuizcard(dto);
        
        return NoContent();
    }
    
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteQuizcard([FromBody] DeleteQuizcardDto dto)
    {
        await quizboardRepository.DeleteQuizcardById(dto);
        
        return NoContent();
    }
    
}