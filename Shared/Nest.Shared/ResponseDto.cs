namespace Nest.Shared;

public class ResponseDto
{
    public bool IsSuccess { get; set; }

    public int StatusCode { get; set; }

    public string[]? Errors { get; set; }

    public string? Message { get; set; }

    public object? Data { get; set; }
}