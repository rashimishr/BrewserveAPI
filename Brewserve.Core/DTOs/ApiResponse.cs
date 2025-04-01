
namespace Brewserve.Core.DTOs
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }

        public ApiResponse(T data)
        {
            IsSuccess = true;
            Data = data;
            Message = Message; 
            Errors = new ();
        }

        public ApiResponse(List<string> errors)
        {
            IsSuccess = true;
            Message = Message;
            Data = default;
            Errors = new();
        }
    }
}
