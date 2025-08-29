namespace CastAmNow.Core.Models
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public bool? Error { get; set; }
        public Response(T data, string message, bool error)
        {
            Data = data;
            Message = message;
            Error = error;
        }
        public Response(string message, bool error)
        {
            Data = default;
            Message = message;
            Error = error;
        }
        public Response()
        {

        }

    }
}
