using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OllamaController : ControllerBase
{
    private readonly OllamaService _ollamaService;

    public OllamaController(OllamaService ollamaService)
    {
        _ollamaService = ollamaService;
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModelsAsync()
    {
        var models = await _ollamaService.GetLocalModelsAsync();
        foreach(var model in models)
        {
            
        }
        return Ok(models);
    }
}