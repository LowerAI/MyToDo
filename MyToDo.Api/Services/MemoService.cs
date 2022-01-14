using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services;

public class MemoService : IMemoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse> AddAsync(MemoDto model)
    {
        try
        {
            var memo = _mapper.Map<Memo>(model);
            memo.CreateDate = DateTime.Now;
            await _unitOfWork.GetRepository<Memo>().InsertAsync(memo);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, memo);
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
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(memo);

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

    public async Task<ApiResponse> GetAllAsync(QueryParameter parameters)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameters.Search) ? true : x.Title.Contains(parameters.Search), pageIndex: parameters.PageIndex, pageSize: parameters.PageSize, orderBy: source => source.OrderByDescending(t => t.CreateDate));

            return new ApiResponse(true, memo);
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
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

            return new ApiResponse(true, memo);
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public async Task<ApiResponse> UpdateAsync(MemoDto entity)
    {
        try
        {
            var dbMemo = _mapper.Map<Memo>(entity);
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbMemo.Id));
            memo.Title = dbMemo.Title;
            memo.Content = dbMemo.Content;
            memo.UpdateDate = DateTime.Now;

            repository.Update(memo);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, memo);
            }

            return new ApiResponse("更新数据失败!");
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }
}