using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using System.Threading.Tasks;

namespace MyToDo.Service;

public interface IToDoService : IBaseService<ToDoDto>
{
    Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter);
}