using MyToDo.Shared;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public interface IBaseService<T>
{
    Task<bool> AddAsync(T entity);

    Task<bool> DeleteAsync(int id);

    Task<IPagedList<T>> GetAllAsync(QueryParameter parameters);

    Task<T> GetSingleAsync(int id);

    Task<bool> UpdateAsync(T entity);
}