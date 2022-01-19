using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared;
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

    public async Task<bool> AddAsync(MemoDto model)
    {
        try
        {
            var memo = _mapper.Map<Memo>(model);
            memo.CreateDate = DateTime.Now;
            await _unitOfWork.GetRepository<Memo>().InsertAsync(memo);

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
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(memo);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IPagedList<MemoDto>> GetAllAsync(QueryParameter parameters)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var memos = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameters.Search) ? true : x.Title.Contains(parameters.Search), pageIndex: parameters.PageIndex, pageSize: parameters.PageSize, orderBy: source => source.OrderByDescending(t => t.CreateDate));
            var memoDtos = _mapper.Map<PagedList<MemoDto>>(memos);
            return memoDtos;
        }
        catch
        {
            throw;
        }
    }

    public async Task<MemoDto> GetSingleAsync(int id)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            var model = _mapper.Map<MemoDto>(memo);
            return model;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(MemoDto entity)
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

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }
}