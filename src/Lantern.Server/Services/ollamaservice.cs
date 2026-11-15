using Lantern.Shared;
using OllamaSharp;

public class OllamaService()
{

    private  readonly OllamaApiClient ollamaclient = new OllamaApiClient("http://192.168.1.9:11434");

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

}
