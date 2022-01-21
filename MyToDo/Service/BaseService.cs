using MyToDo.Services;
using MyToDo.Shared;
using Newtonsoft.Json;
using QueryParameter = MyToDo.Shared.Parameters.QueryParameter;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace MyToDo.Service;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName;

    public BaseService(HttpRestClient client, string serviceName)
    {
        _client = client;
        _serviceName = serviceName;
    }

    public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Post;
        request.Route = $"api/{_serviceName}";
        request.Parameter = entity;
        request.Place = PlaceToAddParameter.Body;
        var response = await _client.ExecuteAsync(request);
        var reps = ParseResponse<TEntity>(response);
        return reps;
    }

    public async Task<ApiResponse> DeleteAsync(int Id)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Delete;
        request.Route = $"api/{_serviceName}/{Id}";
        request.Place = PlaceToAddParameter.UrlSegment;
        var response = await _client.ExecuteAsync(request);
        var reps = ParseResponse(response);
        return reps;
    }

    public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Get;
        string seachField = string.IsNullOrWhiteSpace(parameter.Search) ? "" : $"&Search={parameter.Search}";
        request.Route = $"api/{_serviceName}?PageIndex={parameter.PageIndex}&PageSize={parameter.PageSize}{seachField}";
        request.Place = PlaceToAddParameter.Form;
        var response = await _client.ExecuteAsync(request);
        var reps = ParseResponse<PagedList<TEntity>>(response);
        return reps;
    }

    public async Task<ApiResponse<TEntity>> GetFirstOrDefaultAsync(int id)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Get;
        request.Route = $"api/{_serviceName}/{id}";
        request.Place = PlaceToAddParameter.UrlSegment;
        var response = await _client.ExecuteAsync(request);
        var reps = ParseResponse<TEntity>(response);
        return reps;
    }

    public async Task<ApiResponse> UpdateAsync(TEntity entity)
    {
        BaseRequest request = new BaseRequest();
        request.Method = Method.Put;
        request.Route = $"api/{_serviceName}";
        request.Parameter = entity;
        request.Place = PlaceToAddParameter.Body;
        var response = await _client.ExecuteAsync(request);
        var reps = ParseResponse(response);
        return reps;
    }

    public ApiResponse ParseResponse(RestResponse response)
    {
        (Method method, HttpStatusCode code, string contentType, string content) = (response.Request.Method, response.StatusCode, response.ContentType, response.Content);
        ApiResponse reps = new();
        if (response.ResponseStatus == ResponseStatus.Error)
        {
            reps.Status = false;
            reps.Message = response.ErrorMessage ?? response.StatusDescription;
            return reps;
        }
        switch (method)
        {
            case Method.Post:
                reps.Status = code == HttpStatusCode.Created;
                break;
            case Method.Delete:
                reps.Status = code == HttpStatusCode.NoContent;
                break;
            case Method.Get:
                reps.Status = code == HttpStatusCode.OK;
                break;
            case Method.Put:
                reps.Status = code == HttpStatusCode.ResetContent;
                break;
        }

        if (contentType == "text/plain")
        {
            reps.Message = content!;
        }

        return reps;
    }

    public ApiResponse<T> ParseResponse<T>(RestResponse response)
    {
        (Method method, HttpStatusCode code, string contentType, string content) = (response.Request.Method, response.StatusCode, response.ContentType, response.Content);
        ApiResponse<T> reps = new();
        if (response.ResponseStatus == ResponseStatus.Error)
        {
            reps.Status = false;
            reps.Message = response.ErrorMessage!;
            return reps;
        }
        switch (method)
        {
            case Method.Post:
                reps.Status = code == HttpStatusCode.Created;
                break;
            case Method.Delete:
                reps.Status = code == HttpStatusCode.Gone;
                break;
            case Method.Get:
                reps.Status = code == HttpStatusCode.OK;
                break;
            case Method.Put:
                reps.Status = code == HttpStatusCode.Moved;
                break;
        }

        if (contentType == "text/plain")
        {
            reps.Message = content!;
        }
        else if (contentType == "application/json" && !string.IsNullOrWhiteSpace(response.Content))
        {
            reps.Result = JsonConvert.DeserializeObject<T>(response.Content)!;
        }

        return reps;
    }
}