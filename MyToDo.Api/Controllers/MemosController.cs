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
public class MemosController : ControllerBase
{
    private readonly IMemoService _service;

    public MemosController(IMemoService service)
    {
        _service = service;
    }

    // POST api/Memos
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MemoDto model)
    {
        var result = await _service.AddAsync(model);
        if (!result)
        {
            return StatusCode(500, "添加备忘录失败！");
        }
        return StatusCode(201);
    }

    // DELETE api/Memos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
        {
            return StatusCode(500, "删除备忘录失败！");
        }
        return StatusCode(204);
    }

    // GET api/Memos/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetSingleAsync(id);
        if (result == null)
        {
            return NotFound(); // StatusCode:404
        }
        return Ok(result); // StatusCode:200
    }

    // GET api/Memos
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ToDoParameter param)
    {
        var result = await _service.GetAllAsync(param);
        if (result == null)
        {
            return NotFound(); // StatusCode:404
        }
        return Ok(result); // StatusCode:200
    }

    // PUT api/Memos
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] MemoDto model)
    {
        var result = await _service.UpdateAsync(model);
        if (!result)
        {
            return StatusCode(409, "修改待办失败！");
        }
        return StatusCode(205);
    }
}