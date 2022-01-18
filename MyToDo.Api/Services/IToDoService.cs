using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public interface IToDoService : IBaseService<ToDoDto>
{
    Task<IPagedList<ToDoDto>> GetAllAsync(ToDoParameter parameters);

    Task<SummaryDto> GetSummaryAsync();
}