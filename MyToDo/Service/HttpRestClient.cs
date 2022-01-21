using RestSharp;
using System.Collections.Generic;
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
            switch (baseRequest.Place)
            {
                case PlaceToAddParameter.Body:
                    //request.AddBody(JsonConvert.SerializeObject(baseRequest.Parameter), "application/json");
                    request.AddJsonBody(baseRequest.Parameter);
                    break;
                case PlaceToAddParameter.Form:
                    var dict1 = baseRequest.Parameter as Dictionary<string, string>;
                    foreach (var item in dict1)
                    {
                        request.AddParameter(item.Key, item.Value, ParameterType.RequestBody);
                    }
                    //request.AddHeader("Content-Type", "multipart/form-data");//如果使用了AddFile会自动添加否则请手动设置
                    break;
                case PlaceToAddParameter.Header:
                    var dict2 = baseRequest.Parameter as Dictionary<string, string>;
                    foreach (var item in dict2)
                    {
                        request.AddHeader(item.Key, item.Value);
                    }
                    break;
                case PlaceToAddParameter.None:
                    break;
                case PlaceToAddParameter.Query:
                    var dict3 = baseRequest.Parameter as Dictionary<string, string>;
                    foreach (var item in dict3)
                    {
                        request.AddQueryParameter(item.Key, item.Value);
                    }
                    break;
                case PlaceToAddParameter.UrlSegment:
                    string segment = baseRequest.Parameter?.ToString();
                    baseRequest.Route += $"/{segment}";
                    break;
            }
        }
        client.BuildUri(request);
        var response = await client.ExecuteAsync(request);
        //var result = JsonConvert.DeserializeObject<ApiResponse>(response.Content!);
        return response;
    }
}