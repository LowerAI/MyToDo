using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels;

public class ToDoViewModel : NavigateViewModel
{
    private readonly IToDoService _service;

    public ToDoViewModel(IToDoService service, IContainerProvider provider) : base(provider)
    {
        _service = service;
        ToDoDtos = new();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
        DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
    }

    private string search;
    /// <summary>
    /// 搜索条件
    /// </summary>
    public string Search
    {
        get { return search; }
        set { search = value; RaisePropertyChanged(); }
    }

    private int selectedIndex;
    /// <summary>
    /// 下拉列表选中状态时
    /// </summary>
    public int SelectedIndex
    {
        get { return selectedIndex; }
        set { selectedIndex = value; RaisePropertyChanged(); }
    }

    private bool isRightDrawerOpen;
    /// <summary>
    /// 右侧添加窗口是否展开
    /// </summary>
    public bool IsRightDrawerOpen
    {
        get { return isRightDrawerOpen; }
        set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }

    private ToDoDto _currrentDto;
    /// <summary>
    /// 新增/修改的对象
    /// </summary>
    public ToDoDto CurrentDto
    {
        get { return _currrentDto; }
        set { _currrentDto = value; RaisePropertyChanged(); }
    }

    public DelegateCommand<string> ExecuteCommand { get; private set; }
    public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
    public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }

    private ObservableCollection<ToDoDto> toDoDtos;
    
    public ObservableCollection<ToDoDto> ToDoDtos
    {
        get { return toDoDtos; }
        set { toDoDtos = value; RaisePropertyChanged(); }
    }

    private async void GetDataAsync()
    {
        try
        {
            UpdateLoading(true);

            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;

            var todoResult = await _service.GetAllFilterAsync(new ToDoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = Status
            });

            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    ToDoDtos.Add(item);
                }
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        GetDataAsync();
    }

    /// <summary>
    /// 新增的前置条件
    /// </summary>
    private void Add()
    {
        CurrentDto = new ToDoDto();
        IsRightDrawerOpen = true;
    }

    private void Execute(string obj)
    {
        switch (obj)
        {
            case "新增": Add(); break;
            case "查询": GetDataAsync(); break;
            case "保存": Save(); break;
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    private async void Delete(ToDoDto obj)
    {
        var deleteResult = await _service.DeleteAsync(obj.Id);
        if (deleteResult.Status)
        {
            var model = ToDoDtos.FirstOrDefault(x => x.Id == obj.Id);
            if (model != null)
            {
                ToDoDtos.Remove(model);
            }
        }
    }

    /// <summary>
    /// 保存新增或修改的待办内容
    /// </summary>
    private async void Save()
    {
        if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
        {
            return;
        }

        UpdateLoading(true);

        try
        {
            if (CurrentDto.Id > 0)
            {
                var updateResult = await _service.UpdateAsync(CurrentDto);
                if (updateResult.Status)
                {
                    var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                    if (todo != null)
                    {
                        todo.Title = CurrentDto.Title;
                        todo.Content = CurrentDto.Content;
                        todo.Status = CurrentDto.Status;
                    }
                }
            }
            else
            {
                var addResult = await _service.AddAsync(CurrentDto);
                if (addResult.Status)
                {
                    ToDoDtos.Add(addResult.Result);
                    IsRightDrawerOpen = false;
                }
            }
        }
        catch { }
        finally
        {
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="obj"></param>
    private async void Selected(ToDoDto obj)
    {
        try
        {
            UpdateLoading(true);
            var todoResult = await _service.GetFirstOrDefaultAsync(obj.Id);

            if (todoResult.Status)
            {
                CurrentDto = todoResult.Result;
                IsRightDrawerOpen = true;
            }
        }
        catch
        { }
        finally
        {
            UpdateLoading(false);
        }
    }
}