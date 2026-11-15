using Lantern.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using OllamaSharp;

public class OllamaService()
{

    private readonly OllamaApiClient ollamaclient = new OllamaApiClient("http://192.168.1.9:11434");

    //gets all the models from the ollama server
    public async Task<IEnumerable<OllamaModelResponseDTO>> GetLocalModelsAsync()
    {
        var models = await ollamaclient.ListLocalModelsAsync();
        var result = models.Select(x => new OllamaModelResponseDTO()
        {
            Name = x.Name,
            Size = x.Size,
            ModelParameterSize = x.Details.ParameterSize,
            ModelQuantizationLevel = x.Details.QuantizationLevel,

        });
        return result;
    }


    public async Task<bool> IsModelAvailableAsync(string userRequestedModel)
    {
        if (string.IsNullOrWhiteSpace(userRequestedModel))
        {
            return false;
        }

        var models = await GetLocalModelsAsync();

        foreach (var model in models)
        {
            if (model.Name == userRequestedModel)
            {
                return true;
            }
        }
        return false;
    }

}
