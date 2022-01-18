using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;

using System.Security.Principal;

namespace MyToDo.Api.Services;

public class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// 添加帐户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> AddUserAsync(UserDto user)
    {
        try
        {
            var model = _mapper.Map<User>(user);

            var repository = _unitOfWork.GetRepository<User>();

            model.CreateDate = DateTime.Now;
            await repository.InsertAsync(model);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// 查询帐号Id是否存在
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<User> IsAccountExistAsync(string account)
    {
        if (string.IsNullOrWhiteSpace(account))
        {
            throw new ArgumentNullException(nameof(account));
        }
        return await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: x => (x.Account.Equals(account)));
    }

    /// <summary>
    /// 查询帐户是否存在
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<User> IsUserExistAsync(string account, string password)
    {
        if (string.IsNullOrWhiteSpace(account))
        {
            throw new ArgumentNullException(nameof(account));
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }
        return await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: x => (x.Account.Equals(account)) && (x.Password.Equals(password)));
    }
}