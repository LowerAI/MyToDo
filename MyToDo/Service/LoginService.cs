using MyToDo.Services;
using MyToDo.Shared.Dtos;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyToDo.Service;

public class LoginService : BaseService<UserDto>, ILoginService
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName = "Login";

    public LoginService(HttpRestClient client) : base(client, "ToDos")
    {
        _client = client;
    }

    public async Task<ApiResponse<UserDto>> LoginAsync(UserDto dto)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Get;
        request.Route = $"api/{_serviceName}";
        request.Parameter = new Dictionary<string, string> { { "password", dto.Password }, { "account", dto.Account } };
        request.Place = PlaceToAddParameter.Form;
        var response = await _client.ExecuteAsync(request);
        var reps = base.ParseResponse<UserDto>(response);
        return reps;
    }

    public async Task<ApiResponse> RegisterAsync(UserDto dto)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Post;
        request.Route = $"api/{_serviceName}";
        request.Parameter = dto;
        request.Place = PlaceToAddParameter.Body;
        var response = await _client.ExecuteAsync(request);
        var reps = base.ParseResponse(response);
        return reps;
    }
}