using ControlSystem.Domain.Enums;

namespace ControlSystem.Domain.Response
{
    public class BaseResponse<T> : IErrorResponse<T>
    {
        public string? Description { get; set; }

        public StatusCode StatusCode { get; set; }
        public T? Data { get; set; }
    }
}
