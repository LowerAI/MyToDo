using Newtonsoft.Json;
using RestSharp;
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

    public async Task<RestResponse> ExecuteAsync(BaseRequest baseRequest)
    {
        var request = new RestRequest(baseRequest.Route, baseRequest.Method);
        //request.AddHeader("Accept", baseRequest.ContentType);

        if (baseRequest.Parameter != null)
        {
            request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter),ParameterType.RequestBody);
        }
        client.BuildUri(request);
        var response = await client.ExecuteAsync(request);
        //var result = JsonConvert.DeserializeObject<ApiResponse>(response.Content!);
        return response;
    }

    //public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
    //{
    //    var request = new RestRequest(baseRequest.Route, baseRequest.Method);
    //    request.AddHeader("Accept", baseRequest.ContentType);
    //    //request.AddHeader("charset", "utf-8");

    //    if (baseRequest.Parameter != null)
    //    {
    //        request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
    //    }
    //    client.BuildUri(request);
    //    var response = await client.ExecuteAsync<T>(request);
    //    //var result = JsonFormatter.DeserializeObject<ApiResponse<T>>(response.Content);

    //    // 此处的“ApiResponse<T>”在运行时被替换为“ApiResponse<PagedList<ToDoDto>>”/“ApiResponse<PagedList<MemoDto>>”/“ApiResponse<PagedList<UserDto>>”，对于这种多层嵌套的json反序列化为指定类型的对象经测试只有Newtonsoft.Json可以正确解析
    //    var result = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content!);
    //    //var result = JsonSerializer.Deserialize<ApiResponse<T>>(response.Content);
    //    return result!;
    //}

    //public async Task<T> ExecuteAsync1<T>(BaseRequest baseRequest)
    //{
    //    var request = new RestRequest(baseRequest.Route, baseRequest.Method);
    //    request.AddHeader("Accept", baseRequest.ContentType);
    //    //request.AddHeader("charset", "utf-8");

    //    if (baseRequest.Parameter != null)
    //    {
    //        request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
    //    }
    //    client.BuildUri(request);
    //    var response = await client.ExecuteAsync<T>(request);
    //    //var result = JsonFormatter.DeserializeObject<ApiResponse<T>>(response.Content);

    //    // 此处的“ApiResponse<T>”在运行时被替换为“ApiResponse<PagedList<ToDoDto>>”/“ApiResponse<PagedList<MemoDto>>”/“ApiResponse<PagedList<UserDto>>”，对于这种多层嵌套的json反序列化为指定类型的对象经测试只有Newtonsoft.Json可以正确解析
    //    var result = JsonConvert.DeserializeObject<T>(response.Content!);
    //    //var result = JsonSerializer.Deserialize<ApiResponse<T>>(response.Content);
    //    return result!;
    //}
}