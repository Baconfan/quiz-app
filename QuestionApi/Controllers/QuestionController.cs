using Microsoft.AspNetCore.Mvc;
using QuestionApi.Models;
using QuestionApi.Persistence;

namespace QuestionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController(IQuestionRepository questionRepository) : ControllerBase
{
    private readonly IQuestionRepository _questionRepository = questionRepository;

    [HttpGet]
    public async Task<List<Question>> GetQuestions()
    {
        var  questions = await _questionRepository.GetQuestions();
        
        return questions;
    }
}