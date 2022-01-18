using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels;

/// <summary>
/// 备忘录VM类
/// </summary>
public class MemoViewModel : NavigationViewModel
{
    private readonly IMemoService _service;
    private readonly IDialogHostService dialogHost;

    public MemoViewModel(IMemoService service, IContainerProvider provider) : base(provider)
    {
        _service = service;
        MemoDtos = new();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        SelectedCommand = new DelegateCommand<MemoDto>(Selected);
        DeleteCommand = new DelegateCommand<MemoDto>(Delete);
        dialogHost = provider.Resolve<IDialogHostService>();
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

    private MemoDto _currrentDto;
    /// <summary>
    /// 新增/修改的对象
    /// </summary>
    public MemoDto CurrentDto
    {
        get { return _currrentDto; }
        set { _currrentDto = value; RaisePropertyChanged(); }
    }

    public DelegateCommand<string> ExecuteCommand { get; private set; }
    public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
    public DelegateCommand<MemoDto> DeleteCommand { get; private set; }

    private ObservableCollection<MemoDto> memoDtos;

    public ObservableCollection<MemoDto> MemoDtos
    {
        get { return memoDtos; }
        set { memoDtos = value; RaisePropertyChanged(); }
    }

    private async void GetDataAsync()
    {
        UpdateLoading(true);

        var memoResult = await _service.GetAllAsync(new ToDoParameter()
        {
            PageIndex = 0,
            PageSize = 100,
            Search = Search
        });

        if (memoResult.Status)
        {
            MemoDtos.Clear();
            foreach (var item in memoResult.Result.Items)
            {
                MemoDtos.Add(item);
            }
        }

        UpdateLoading(false);
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
        CurrentDto = new MemoDto();
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
    private async void Delete(MemoDto obj)
    {
        try
        {
            var dialogResult = await dialogHost.Question("温馨提示", $"确认删除备忘录:{obj.Title}？");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
            {
                return;
            }

            UpdateLoading(true);

            var deleteResult = await _service.DeleteAsync(obj.Id);
            if (deleteResult.Status)
            {
                var model = MemoDtos.FirstOrDefault(x => x.Id == obj.Id);
                if (model != null)
                {
                    MemoDtos.Remove(model);
                }
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 保存新增或修改的备忘录内容
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
                    var memo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                    if (memo != null)
                    {
                        memo.Title = CurrentDto.Title;
                        memo.Content = CurrentDto.Content;
                    }
                }
            }
            else
            {
                var addResult = await _service.AddAsync(CurrentDto);
                if (addResult.Status)
                {
                    MemoDtos.Add(addResult.Result);
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
    private async void Selected(MemoDto obj)
    {
        try
        {
            UpdateLoading(true);
            var memoResult = await _service.GetFirstOrDefaultAsync(obj.Id);

            if (memoResult.Status)
            {
                CurrentDto = memoResult.Result;
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