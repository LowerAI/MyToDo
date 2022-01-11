﻿using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Parameters;

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
        request.Method = RestSharp.Method.Put;
        request.Route = $"api/{_serviceName}";
        request.Parameter = entity;
        return await _client.ExecuteAsync<TEntity>(request);
    }

    public async Task<ApiResponse> DeleteAsync(int Id)
    {
        BaseRequest request = new BaseRequest();
        request.Method = RestSharp.Method.Delete;
        request.Route = $"api/{_serviceName}/{Id}";
        return await _client.ExecuteAsync(request);
    }

    public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameters parameter)
    {
        BaseRequest request = new BaseRequest();
        request.Method = RestSharp.Method.Get;
        string seachField = string.IsNullOrWhiteSpace(parameter.Search) ? "" : $"&Search={parameter.Search}";
        request.Route = $"api/{_serviceName}?PageIndex={parameter.PageIndex}&PageSize={parameter.PageSize}{seachField}";
        return await _client.ExecuteAsync<PagedList<TEntity>>(request);
    }

    public async Task<ApiResponse<TEntity>> GetFirstOrDefaultAsync(int id)
    {
        BaseRequest request = new BaseRequest();
        request.Method = RestSharp.Method.Get;
        request.Route = $"api/{_serviceName}/{id}";
        return await _client.ExecuteAsync<TEntity>(request);
    }

    public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
    {
        BaseRequest request = new BaseRequest();
        request.Method = RestSharp.Method.Patch;
        request.Route = $"api/{_serviceName}";
        request.Parameter = entity;
        return await _client.ExecuteAsync<TEntity>(request);
    }
}