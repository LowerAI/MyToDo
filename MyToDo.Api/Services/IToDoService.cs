using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public interface IToDoService: IBaseService<ToDoDto>
{
    Task<ApiResponse> GetAllAsync(ToDoParameter parameters);
}