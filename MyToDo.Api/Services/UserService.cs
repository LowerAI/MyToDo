using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> AddAsync(UserDto model)
    {
        try
        {
           var user = _mapper.Map<User>(model);

            await _unitOfWork.GetRepository<User>().InsertAsync(user);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate:x => x.Id.Equals(id));
            repository.Delete(user);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IPagedList<UserDto>> GetAllAsync(QueryParameter parameters)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<User>();
            var users = await repository.GetPagedListAsync();
            var userDtos = _mapper.Map<IPagedList<UserDto>>(users);
            return userDtos;
        }
        catch
        {
            throw;
        }
    }

    public async Task<UserDto> GetSingleAsync(int id)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            var model = _mapper.Map<UserDto>(user);
            return model;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UserDto entity)
    {
        try
        {
            var dbUser = _mapper.Map<User>(entity);
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbUser.Id));
            user.Account = dbUser.Account;
            user.UserName = dbUser.UserName;
            user.UpdateDate = DateTime.Now;

            repository.Update(user);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }
}