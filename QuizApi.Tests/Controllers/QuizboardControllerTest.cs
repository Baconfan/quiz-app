using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Controllers;
using Xunit;
using Moq;
using QuizApi.Models;
using QuizApi.Persistence;
using AutoFixture;
using Shouldly;

namespace QuizApi.Tests.Controllers;

[TestSubject(typeof(QuizboardController))]
public class QuizboardControllerTest
{
    private readonly QuizboardController _controller;
    private readonly Mock<IQuizboardRepository> _quizboardRepositoryMock;
    
    public QuizboardControllerTest()
    {
        _quizboardRepositoryMock = new Mock<IQuizboardRepository>();
        
        _controller = new QuizboardController(_quizboardRepositoryMock.Object);
    }
    
    // GetAllQuizboards
    [Fact]
    public async Task GetAllQuizboards_CallsQuizboardRepository()
    {
        await _controller.GetAllQuizboards();
        
        _quizboardRepositoryMock.Verify(q => q.GetAllQuizboards(), Times.Once);
    }

    [Fact]
    public async Task GetAllQuizboards_ReturnsQuizboardDtoFromRepositoryUnmapped()
    {
        var fixture = new Fixture();
        var setupRespositoryOutput = fixture.CreateMany<QuizboardDto>().ToList();
        
        _quizboardRepositoryMock
            .Setup(q => q.GetAllQuizboards())
            .ReturnsAsync(setupRespositoryOutput);
        
        var result = await _controller.GetAllQuizboards();
        
        Assert.Equal(result, setupRespositoryOutput);
    }

    // GetQuizboardById
    [Fact]
    public async Task GetQuizboardById_CallsQuizboardRepository()
    {
        var fixture = new Fixture();
        var quizboardId = new string(fixture.CreateMany<char>().Take(20).ToArray());
        
        await _controller.GetQuizboardById(quizboardId);
        
        _quizboardRepositoryMock.Verify(q => q.GetQuizboardById(quizboardId), Times.Once);
    }
    
    [Fact]
    public async Task GetQuizboardById_ReturnsOkWithMatchingQuizboard()
    {
        var fixture = new Fixture();
        var quizboardId = new string(fixture.CreateMany<char>().Take(20).ToArray());
        var setupOutput = fixture
            .Build<QuizboardDto>()
            .With(q => q.Id, quizboardId)
            .Create();

        _quizboardRepositoryMock
            .Setup(q => q.GetQuizboardById(quizboardId))
            .ReturnsAsync(setupOutput);
        
        var result = await _controller.GetQuizboardById(quizboardId);

        result
            .Result.ShouldBeOfType<OkObjectResult>()
            .Value.ShouldBe(setupOutput);
    }
    
    [Fact]
    public async Task GetQuizboardById_NoQuizboardFound_ReturnsNull()
    {
        var fixture = new Fixture();
        var randomId = new string(fixture.CreateMany<char>().Take(20).ToArray());
        
        _quizboardRepositoryMock
            .Setup(q => q.GetQuizboardById(It.IsAny<string>()))
            .ReturnsAsync((QuizboardDto?)null);
        
        var result = await _controller.GetQuizboardById(randomId);

        result.Result.ShouldBeOfType<NotFoundObjectResult>();
    }
}