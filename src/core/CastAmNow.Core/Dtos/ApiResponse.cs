namespace CastAmNow.Core.Dtos;
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    
    public List<string> Errors { get; set; } = [];
    public ApiResponse() { }
    
    public ApiResponse(T data)
    {
        IsSuccess = true;
        Message = "";
        Data = data;
    }

    public static ApiResponse<T> Ok(T? data = default, string message = "The operation was completed successfully")
        => new()
        {
            Data = data,
            IsSuccess = true,
            Message = message,
        };
    
    public static ApiResponse<T> Failed(string message = "An error occured. Please try again later", T? data = default)
        => new()
        {
            Data = data,
            IsSuccess = false,
            Message = message,
        };
}