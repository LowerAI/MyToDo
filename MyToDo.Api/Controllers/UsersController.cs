using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Controllers;

/// <summary>
/// 登录控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILoginService _service;

    public UsersController(ILoginService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    // GET api/Users
    [HttpGet(Name = nameof(Login))]
    public async Task<IActionResult> Login(string account, string password)
    {
        var user = await _service.IsUserExistAsync(account, password);
        if (user == null)
        {
            return NotFound(); // StatusCode:404
        }
        return Ok(); // StatusCode:200
    }

    // POST api/Users
    [HttpPost(Name = nameof(Register))]
    public async Task<IActionResult> Register([FromBody] UserDto param)
    {
        var user = await _service.IsAccountExistAsync(param.Account);

        if (user != null)
        {
            return StatusCode(500, $"当前帐号:{param.Account}已存在，请重新注册！");
        }

        if (!await _service.AddUserAsync(param))
        {
            return StatusCode(500, "创建帐户失败！");
        }
        return StatusCode(201);
    }
}