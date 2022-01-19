using Newtonsoft.Json;
using RestSharp;

using System.Net;
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
        //request.AddHeader("Content-Type", baseRequest.ContentType);

        if (baseRequest.Parameter != null)
        {
            switch (baseRequest.Method)
            {
                case Method.Delete:
                case Method.Get:
                    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                    break;
                case Method.Post:
                case Method.Put:
                    request.AddBody(JsonConvert.SerializeObject(baseRequest.Parameter), "application/json");
                    break;
            }
        }
        client.BuildUri(request);
        var response = await client.ExecuteAsync(request);
        //var result = JsonConvert.DeserializeObject<ApiResponse>(response.Content!);
        return response;
    }
}