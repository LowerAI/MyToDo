using AutoMapper;

using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using System.Collections.ObjectModel;

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

    public async Task<bool> AddAsync(ToDoDto model)
    {
        try
        {
            var todo = _mapper.Map<ToDo>(model);
            todo.CreateDate = DateTime.Now;
            await _unitOfWork.GetRepository<ToDo>().InsertAsync(todo);

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
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(todo);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IPagedList<ToDoDto>> GetAllAsync(QueryParameter parameter)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todos = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search), pageIndex: parameter.PageIndex, pageSize: parameter.PageSize, orderBy: source => source.OrderByDescending(t => t.CreateDate));
            var todoDtos = _mapper.Map<IPagedList<ToDoDto>>(todos);
            return todoDtos;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IPagedList<ToDoDto>> GetAllAsync(ToDoParameter parameter)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todos = await repository.GetPagedListAsync(predicate: x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search)) && (parameter.Status == null ? true : x.Status.Equals(parameter.Status)), pageIndex: parameter.PageIndex, pageSize: parameter.PageSize, orderBy: source => source.OrderByDescending(t => t.CreateDate));
            var todoDtos = _mapper.Map<PagedList<ToDoDto>>(todos);
            return todoDtos;
        }
        catch
        {
            throw;
        }
    }

    public async Task<ToDoDto> GetSingleAsync(int id)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            var model = _mapper.Map<ToDoDto>(todo);
            return model;
        }
        catch
        {
            throw;
        }
    }

    public async Task<SummaryDto> GetSummaryAsync()
    {
        try
        {
            // 待办事项结果
            var todos = await _unitOfWork.GetRepository<ToDo>().GetAllAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));

            // 备忘录结果
            var memos = await _unitOfWork.GetRepository<Memo>().GetAllAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));

            SummaryDto summary = new();
            summary.Sum = todos.Count; // 汇总待办事项数量
            summary.CompletedCount = todos.Where(t => t.Status == 1).Count(); // 统计完成数量
            summary.CompletedRatio = (summary.CompletedCount / summary.Sum).ToString("0%"); // 统计完成率
            summary.MemoCount = memos.Count; // 汇总备忘录数量

            summary.ToDoList = new ObservableCollection<ToDoDto>(_mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
            summary.MemoList = new ObservableCollection<MemoDto>(_mapper.Map<List<MemoDto>>(memos));

            return summary;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ToDoDto entity)
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

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch
        {
            throw;
        }
    }
}