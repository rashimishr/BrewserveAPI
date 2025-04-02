
using Azure.Core;

namespace Brewserve.Core.Payloads
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse(T data, string message = "Request Successful")
        {
            IsSuccess = true;
            Data = data;
            Message = message; 
        }

        public ApiResponse(string errorMessage)
        {
            IsSuccess = false;
            Message = "Request Failed";
            Errors = [errorMessage];
        }

        public ApiResponse(List<string> errorMessages)
        {
            IsSuccess = false;
            Message = "Validation failed";
            Errors = errorMessages;
        }
    }
}
