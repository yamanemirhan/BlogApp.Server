namespace BlogApp.Application.DTOs.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; } 
    }
}
