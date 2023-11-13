namespace ControlSystem.Domain.Response
{
    public interface IBaseResponse<T>
    {
        string Description { get; }
        T Data { get; set; }
    }
}
