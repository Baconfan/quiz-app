using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Controllers;
using Xunit;

namespace QuizApi.Tests.Controllers;

[TestSubject(typeof(HealthCheckController))]
public class HealthCheckControllerTest
{
    private readonly HealthCheckController _controller;

    public HealthCheckControllerTest()
    {
        _controller =  new HealthCheckController();
    }


    [Fact]
    public void ReturnsOkResult()
    {
        var result = _controller.ReturnOk();
        
        Assert.IsType<OkObjectResult>(result);
    }
}