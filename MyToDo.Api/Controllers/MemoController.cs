﻿using Microsoft.AspNetCore.Mvc;

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

    // POST api/Memo
    [HttpPost]
    public async Task<ApiResponse> Add([FromBody] MemoDto model) => await _service.AddAsync(model);

    // DELETE api/Memo/5
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id) => await _service.DeleteAsync(id);

    // GET api/Memo/5
    [HttpGet("{id}")]
    public async Task<ApiResponse> Get(int id) => await _service.GetSingleAsync(id);

    // GET api/Memo
    [HttpGet]
    public async Task<ApiResponse> GetAll([FromQuery] ToDoParameter param) => await _service.GetAllAsync(param);

    // PUT api/Memo
    [HttpPut]
    public async Task<ApiResponse> Update([FromBody] MemoDto model) => await _service.UpdateAsync(model);
}