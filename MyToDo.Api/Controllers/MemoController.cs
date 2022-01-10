using Microsoft.AspNetCore.Mvc;

using MyToDo.Api.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers;

/// <summary>
/// 备忘录控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MemoController : ControllerBase
{
    private readonly IMemoService _service;

    public MemoController(IMemoService service)
    {
        _service = service;
    }

    // GET api/Memo/5
    [HttpGet("{id}")]
    public async Task<ApiResponse> Get(int id) => await _service.GetSingleAsync(id);

    // GET api/Memo
    [HttpGet]
    public async Task<ApiResponse> GetAll([FromQuery] QueryParameters param) => await _service.GetAllAsync(param);

    // PUT api/Memo
    [HttpPut]
    public async Task<ApiResponse> Add([FromBody] MemoDto model) => await _service.AddAsync(model);

    // PATCH api/Memo
    [HttpPatch]
    public async Task<ApiResponse> Update([FromBody]MemoDto model) => await _service.UpdateAsync(model);

    // DELETE api/Memo/5
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id) => await _service.DeleteAsync(id);
}