using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Dtos.Input;
using QuizApi.Dtos.Output;
using QuizApi.Facades;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class QuizcardController(
    IQuizboardRepository quizboardRepository, 
    IImageUploadFacade imageUploadFacade) : BaseApiController
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };
    
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
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<QuizImageDto>> AddImage([FromForm]UploadRequest uploadRequest)
    {
        if (uploadRequest.File is null || uploadRequest.File.Length == 0)
            return BadRequest("No file uploaded.");

        if (string.IsNullOrWhiteSpace(uploadRequest.MetaData))
            return BadRequest("Missing metadata.");
        
        ImageUploadToGamecardDto metaData;
        
        try
        {
            metaData = JsonSerializer.Deserialize<ImageUploadToGamecardDto>(uploadRequest.MetaData, JsonOptions)
                   ?? throw new JsonException("Metadata is null after deserialization.");
        }
        catch (JsonException ex)
        {
            return BadRequest($"Invalid metadata JSON: {ex.Message}");
        }
        
        await using var stream = uploadRequest.File.OpenReadStream();
        var x = new ImageUploadForFacade
        {
            ToBeUploadedFile = stream,
            FileName = uploadRequest.File.FileName,
            QuizcardId = metaData.QuizcardId,
            ImageAssigment = metaData.ImageAssignment,
            Position = metaData.ArrayPosition
        };
        
        var result = await imageUploadFacade.UploadAndPersistImage(x);
        
        return result is null 
            ? NoContent() 
            : Ok(result);
    }
}