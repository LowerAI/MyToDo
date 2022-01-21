using MyToDo.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Threading.Tasks;

namespace MyToDo.Service;

public class ToDoService : BaseService<ToDoDto>, IToDoService
{
    private readonly HttpRestClient _client;

    public ToDoService(HttpRestClient client) : base(client, "ToDos")
    {
        _client = client;
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
        string statusField = parameter.Status == null ? "" : $"&Status={parameter.Status}";
        request.Route = $"api/ToDos?PageIndex={parameter.PageIndex}&PageSize={parameter.PageSize}{seachField}{statusField}";
        request.Place = PlaceToAddParameter.Form;
        var response = await _client.ExecuteAsync(request);
        var reps = base.ParseResponse<PagedList<ToDoDto>>(response);
        return reps;
    }

    public async Task<ApiResponse<SummaryDto>> GetSummaryAsync()
    {
        BaseRequest request = new BaseRequest();
        request.Method = RestSharp.Method.Get;
        request.Route = $"api/Summarys";
        request.Place = PlaceToAddParameter.None;
        var response = await _client.ExecuteAsync(request);
        var reps = base.ParseResponse<SummaryDto>(response);
        return reps;
    }
}