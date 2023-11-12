namespace ControlSystem.Domain.Response
{
    public interface IErrorResponse<T>
    {
        string Description { get; }
        T Data { get; set; }
    }
}
