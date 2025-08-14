using Microsoft.AspNetCore.Mvc;
using QuizApi.Entities;
using QuizApi.Enums;
using QuizApi.Facades;
using QuizApi.InputDto;
using QuizApi.Interfaces;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class QuizcardController(
    IQuizboardRepository quizboardRepository, 
    IImageService imageService, 
    IImageUploadFacade imageUploadFacade) : BaseApiController
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
    public async Task<ActionResult<QuizImageDto>> AddImage([FromForm]ImageUploadToGamecardDto imageUpload)
    {
        await using var stream = imageUpload.ToBeUploadedFile.OpenReadStream();
        var x = new ImageUploadForFacade
        {
            ToBeUploadedFile = stream,
            FileName = imageUpload.ToBeUploadedFile.FileName,
            QuizcardId = imageUpload.QuizcardId,
            ImageAssigment = imageUpload.ImageAssigment
        };
        
        var result = await imageUploadFacade.UploadAndPersistImage(x);
        
        return result is null 
            ? NoContent() 
            : Ok(result);
    }
}