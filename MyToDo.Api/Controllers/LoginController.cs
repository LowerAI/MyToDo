using Microsoft.AspNetCore.Mvc;

using MyToDo.Api.Services;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Controllers;

/// <summary>
/// 登录控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _service;

    public LoginController(ILoginService service)
    {
        _service = service;
    }

    // GET api/Login
    [HttpGet]
    public async Task<ApiResponse> Get(string account, string password) => await _service.LoginActionAsync(account, password);

    // POST api/Login
    [HttpPost]
    public async Task<ApiResponse> Register([FromBody] UserDto param) => await _service.RegisterAsync(param);
}