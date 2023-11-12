namespace ControlSystem.Domain.Response
{
    public class ErrorResponse<T> : IErrorResponse<T>
    {
        public string Description { get; set; }

        public T Data { get; set; }
    }
}
