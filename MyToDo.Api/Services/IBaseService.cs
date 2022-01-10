using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public interface IBaseService<T>
{
    Task<ApiResponse> GetAllAsync(QueryParameters parameters);

    Task<ApiResponse> GetSingleAsync(int id);

    Task<ApiResponse> AddAsync(T entity);

    Task<ApiResponse> UpdateAsync(T entity);

    Task<ApiResponse> DeleteAsync(int id);
}