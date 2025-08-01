using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Entities;
using QuizApi.Persistence;

namespace QuizApi.Controllers;

public class QuestionController(IQuestionRepository questionRepository) : BaseApiController
{
    [Authorize]
    [HttpGet("all")]
    public async Task<List<Question>> GetQuestions()
    {
        var  questions = await questionRepository.GetQuestions();
        
        return questions;
    }
}