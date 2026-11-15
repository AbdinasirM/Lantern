using Lantern.Shared;
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

        return Ok(models);
    }
    [HttpGet("CheckModelAvailability")]
    public async Task<IActionResult> CheckModelAvailability(string userRequestedModel)
    {
        if (string.IsNullOrWhiteSpace(userRequestedModel))
        {
            return BadRequest("Model name can not be empty.");
        }
        var modelIsAvaile = await _ollamaService.IsModelAvailableAsync(userRequestedModel);

        if (!modelIsAvaile)
        {
            return NotFound("Model is not on the server");
        }
        // set the converstation model 
        return Ok($"{userRequestedModel} is available");

    }




}