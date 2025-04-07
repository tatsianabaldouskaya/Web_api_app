namespace WebApplicationApi.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    Task<T> AddAsync(T model);

    Task<T> UpdateAsync(int id, T model);

    Task<bool> DeleteAsync(int id);
}
