using MyToDo.Api.Context;
using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using System.Threading.Tasks;

namespace MyToDo.Service;

public class ToDoService : BaseService<ToDoDto>, IToDoService
{
    private readonly HttpRestClient client;

    public ToDoService(HttpRestClient client) : base(client, nameof(ToDo))
    {
        this.client = client;
    }

    /// <summary>
    /// 返回指定条件查询的结果
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter)
    {
        BaseRequest request = new BaseRequest();
        request.Method = RestSharp.Method.Get;
        string seachField = string.IsNullOrWhiteSpace(parameter.Search) ? "" : $"&Search={parameter.Search}";
        request.Route = $"api/{nameof(ToDo)}?PageIndex={parameter.PageIndex}&PageSize={parameter.PageSize}{seachField}&Status={parameter.Status}";
        return await client.ExecuteAsync<PagedList<ToDoDto>>(request);
    }
}