namespace CastAmNow.Core.Dtos;

public class ValidationErrorResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, string[]>? Errors { get; init; }
    public string? TraceId { get; set; }
}