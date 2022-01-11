using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public class ToDoService : IToDoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse> AddAsync(ToDoDto model)
    {
        try
        {
            var todo = _mapper.Map<ToDo>(model);

            await _unitOfWork.GetRepository<ToDo>().InsertAsync(todo);

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
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(todo);

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
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameters.Search) ? true : x.Title.Contains(parameters.Search), pageIndex: parameters.PageIndex, pageSize: parameters.PageSize, orderBy: source => source.OrderByDescending(t => t.CreateDate));

            return new ApiResponse(true, todo);
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
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

            return new ApiResponse(true, todo);
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public async Task<ApiResponse> UpdateAsync(ToDoDto entity)
    {
        try
        {
            var dbToDo = _mapper.Map<ToDo>(entity);
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbToDo.Id));
            todo.Title = dbToDo.Title;
            todo.Content = dbToDo.Content;
            todo.Status = dbToDo.Status;
            todo.UpdateDate = DateTime.Now;

            repository.Update(todo);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, todo);
            }

            return new ApiResponse("更新数据失败!");
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }
}