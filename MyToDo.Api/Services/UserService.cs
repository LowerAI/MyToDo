﻿using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
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

    public async Task<ApiResponse> AddAsync(UserDto model)
    {
        try
        {
           var user = _mapper.Map<User>(model);

            await _unitOfWork.GetRepository<User>().InsertAsync(user);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, model);
            }

            return new ApiResponse("添加数据失败!");
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate:x => x.Id.Equals(id));
            repository.Delete(user);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, id);
            }

            return new ApiResponse("删除数据失败!");
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public async Task<ApiResponse> GetAllAsync(QueryParameters parameters)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetAllAsync();

            return new ApiResponse(true, user);
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public async Task<ApiResponse> GetSingleAsync(int id)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

            return new ApiResponse(true, user);
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public async Task<ApiResponse> UpdateAsync(UserDto entity)
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

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, user);
            }

            return new ApiResponse("更新数据失败!");
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }
}