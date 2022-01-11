using MyToDo.Api.Services;

using RestSharp;

using Swifter.Json;

using System.Text.Json;
using System.Threading.Tasks;


namespace MyToDo.Service;

public class HttpRestClient
{
    //private readonly string _apiUrl;
    protected readonly RestClient client;

    public HttpRestClient(string apiUrl)
    {
        //_apiUrl = apiUrl;
        client = new RestClient(apiUrl);
    }

    public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
    {
        var request = new RestRequest(baseRequest.Route, baseRequest.Method);
        request.AddHeader("Content-Type", baseRequest.ContentType);

        if (baseRequest.Parameter != null)
        {
            request.AddParameter("param", JsonFormatter.SerializeObject(baseRequest.Parameter),ParameterType.RequestBody);
        }
        client.BuildUri(request);
        var response = await client.ExecuteAsync(request);
        return JsonFormatter.DeserializeObject<ApiResponse>(response.Content);
    }

    public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
    {
        var request = new RestRequest(baseRequest.Route, baseRequest.Method);
        request.AddHeader("Content-Type", baseRequest.ContentType);

        if (baseRequest.Parameter != null)
        {
            request.AddParameter("param", JsonFormatter.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
        }
        client.BuildUri(request);
        var response = await client.ExecuteAsync(request);
        var result = JsonFormatter.DeserializeObject<ApiResponse<T>>(response.Content);
        //var result = JsonSerializer.Deserialize<ApiResponse<T>>(response.Content);
        return result;
    }
}