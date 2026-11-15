namespace Lantern.Shared;

public class OllamaModelResponseDTO
{
    public required string Name {get; set;}
    public long Size {get; set;}
    public string? ModelParameterSize {get;set;}
    public string? ModelQuantizationLevel {get;set;}

}
