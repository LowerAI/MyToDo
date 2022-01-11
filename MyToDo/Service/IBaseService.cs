using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Parameters;

using System.Threading.Tasks;

namespace MyToDo.Service;

public interface IBaseService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> AddAsync(TEntity entity);

    Task<ApiResponse> DeleteAsync(int Id);

    Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameters parameter);

    Task<ApiResponse<TEntity>> GetFirstOrDefaultAsync(int id);

    Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);
}