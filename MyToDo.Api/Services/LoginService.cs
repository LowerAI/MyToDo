using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;

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

    public async Task<ApiResponse> LoginActionAsync(string Account, string Password)
    {
        try
        {
            var model = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: x => (x.Account.Equals(Account)) && (x.Password.Equals(Password)));

            if (model == null)
            {
                return new ApiResponse("帐号或密码错误，请重试！");
            }

            return new ApiResponse(true, model);
        }
        catch
        {
            return new ApiResponse(false, "登录失败");
        }
    }

    public async Task<ApiResponse> RegisterAsync(UserDto user)
    {
        try
        {
            var model = _mapper.Map<User>(user);

            var repository = _unitOfWork.GetRepository<User>();

            var userModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));

            if (userModel != null)
            {
                return new ApiResponse($"当前帐号:{model.Account}已存在，请重新注册！");
            }
            model.CreateDate = DateTime.Now;
            await repository.InsertAsync(model);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true,model);
            }

            return new ApiResponse("添加失败！");
        }
        catch
        {
            return new ApiResponse(false, "注册帐号失败！");
        }
    }
}