using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services;

public interface ILoginService
{
    Task<bool> AddUserAsync(UserDto user);

    Task<User> IsAccountExistAsync(string account);

    Task<User> IsUserExistAsync(string account, string password);
}