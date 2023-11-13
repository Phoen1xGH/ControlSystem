namespace ControlSystem.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);

        IQueryable<T> GetAll();

        Task Delete(T entity);

        Task Update(T entity);
    }
}
