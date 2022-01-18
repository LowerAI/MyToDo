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
public class ToDosController : ControllerBase
{
    private readonly IToDoService _service;

    public ToDosController(IToDoService service)
    {
        _service = service;
    }

    // POST api/ToDos
    [HttpPost(Name = nameof(Add))]
    public async Task<IActionResult> Add([FromBody] ToDoDto model)
    {
        var result = await _service.AddAsync(model);
        if (!result)
        {
            return StatusCode(202, "添加待办失败！");
        }
        return StatusCode(201, result);
    }

    // DELETE api/ToDos/5
    [HttpDelete("{id}", Name = nameof(Delete))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
        {
            return StatusCode(304, "删除待办失败！");
        }
        return StatusCode(410);
    }

    // GET api/ToDos/5
    [HttpGet("{id}", Name = nameof(Get))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetSingleAsync(id);
        if (result == null)
        {
            return NotFound(); // StatusCode:404
        }
        return Ok(result); // StatusCode:200
    }

    // GET api/ToDos
    [HttpGet(Name = nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] ToDoParameter param)
    {
        var result = await _service.GetAllAsync(param);
        if (result == null)
        {
            return NotFound(); // StatusCode:404
        }
        return Ok(result); // StatusCode:200
    }

    // PUT api/ToDos
    [HttpPut(Name = nameof(Update))]
    public async Task<IActionResult> Update([FromBody] ToDoDto model)
    {
        var result = await _service.UpdateAsync(model);
        if (!result)
        {
            return StatusCode(304, "修改待办失败！");
        }
        return StatusCode(301);
    }
}