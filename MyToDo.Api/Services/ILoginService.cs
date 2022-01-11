using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services;

public interface ILoginService
{
    Task<ApiResponse> LoginActionAsync(string Account, string Password);

    Task<ApiResponse> RegisterAsync(UserDto user);
}