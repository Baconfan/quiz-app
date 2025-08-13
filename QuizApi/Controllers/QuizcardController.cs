using Microsoft.AspNetCore.Mvc;
using QuizApi.Entities;
using QuizApi.InputDto;
using QuizApi.Interfaces;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class QuizcardController(IQuizboardRepository quizboardRepository, IPhotoService photoService) : BaseApiController
{
    /*
    [HttpPost("new")]
    public async Task<IActionResult> InsertNewQuizcard([FromBody]GamecardDto dto)
    {
        await quizboardRepository.CreateNewQuizcard(dto);

        return NoContent();
    }
    */
    
    [HttpPut("upsert")]
    public async Task<IActionResult> UpsertQuizcard([FromBody] GamecardDto dto)
    {
        await quizboardRepository.UpsertQuizcard(dto);
        
        return NoContent();
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteQuizcard([FromBody] DeleteQuizcardDto dto)
    {
        await quizboardRepository.DeleteQuizcardById(dto);
        
        return NoContent();
    }

    [HttpPost("add-image")]
    public async Task<ActionResult<QuizImage>> AddImage(IFormFile file)
    {
        var result = await photoService.UploadPhotoAsync(file);

        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        var image = new QuizImage
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };

        return image;
    }
}