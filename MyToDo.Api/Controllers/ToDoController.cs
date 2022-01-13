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

    // POST api/ToDo
    [HttpPost]
    public async Task<ApiResponse> Add([FromBody]ToDoDto model) => await _service.AddAsync(model);

    // DELETE api/ToDo/5
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id) => await _service.DeleteAsync(id);

    // GET api/ToDo/5
    [HttpGet("{id}")]
    public async Task<ApiResponse> Get(int id) => await _service.GetSingleAsync(id);

    // GET api/ToDo
    [HttpGet]
    public async Task<ApiResponse> GetAll([FromQuery]ToDoParameter param) => await _service.GetAllAsync(param);

    // PUT api/ToDo
    [HttpPut]
    public async Task<ApiResponse> Update([FromBody]ToDoDto model) => await _service.UpdateAsync(model);
}