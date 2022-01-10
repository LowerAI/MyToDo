using Microsoft.AspNetCore.Mvc;

using MyToDo.Api.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers;

/// <summary>
/// 待办事项控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _service;

    public ToDoController(IToDoService service)
    {
        _service = service;
    }

    // GET api/ToDo/5
    [HttpGet("{id}")]
    public async Task<ApiResponse> Get(int id) => await _service.GetSingleAsync(id);

    // GET api/ToDo
    [HttpGet]
    public async Task<ApiResponse> GetAll([FromQuery]QueryParameters param) => await _service.GetAllAsync(param);

    // PUT api/ToDo
    [HttpPut]
    public async Task<ApiResponse> Add([FromBody]ToDoDto model) => await _service.AddAsync(model);

    // PATCH api/ToDo
    [HttpPatch]
    public async Task<ApiResponse> Update([FromBody] ToDoDto model) => await _service.UpdateAsync(model);

    // DELETE api/ToDo/5
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id) => await _service.DeleteAsync(id);
}