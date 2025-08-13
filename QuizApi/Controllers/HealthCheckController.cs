using Microsoft.AspNetCore.Mvc;

namespace QuizApi.Controllers;

[Route("api/hc")]
[ApiController]
public class HealthCheckController: ControllerBase
{
    [HttpGet]
    public IActionResult ReturnOk() => Ok("Healthy");
}