using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels;

public class IndexViewModel : NavigationViewModel
{
    private readonly IToDoService toDoService;
    private readonly IMemoService memoService;
    private readonly IDialogHostService dialog;
    private readonly IRegionManager regionManager;

    public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
    {
        HeaderTitle = $"你好，{AppSession.UserName}！今天是{DateTime.Now.GetDateTimeFormats('D')[1]}";
        CreateTaskBars();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
        ToDoCompletedCommand = new DelegateCommand<ToDoDto>(Completed);
        EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
        NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
        toDoService = provider.Resolve<IToDoService>();
        memoService = provider.Resolve<IMemoService>();
        regionManager = provider.Resolve<IRegionManager>();

        this.dialog = dialog;
    }

    private void Navigate(TaskBar obj)
    {
        if (string.IsNullOrWhiteSpace(obj.Target))
        {
            return;
        }
        NavigationParameters param = new();
        if (obj.Title == "已完成")
        {
            param.Add("Value", 2);
        }
        regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, param);
    }

    private async void Completed(ToDoDto obj)
    {
        try
        {
            UpdateLoading(true);
            var updateResult = await toDoService.UpdateAsync(obj);
            if (updateResult.Status)
            {
                var todo = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if (todo != null)
                {
                    summary.ToDoList.Remove(todo);
                    summary.CompletedCount += 1;
                    Summary.CompletedRatio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
                    this.Refresh();
                }
            }
            _aggregator.SendMessage("已完成!");
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    #region    属性start
    public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
    public DelegateCommand<ToDoDto> ToDoCompletedCommand { get; private set; }
    public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
    public DelegateCommand<string> ExecuteCommand { get; private set; }
    public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

    private string headerTitle;
    /// <summary>
    /// 标头
    /// </summary>
    public string HeaderTitle
    {
        get { return headerTitle; }
        set { headerTitle = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<TaskBar> taskBars;

    public ObservableCollection<TaskBar> TaskBars
    {
        get { return taskBars; }
        set { taskBars = value; RaisePropertyChanged(); }
    }

    private SummaryDto summary;
    /// <summary>
    /// 首页统计
    /// </summary>
    public SummaryDto Summary
    {
        get { return summary; }
        set { summary = value; RaisePropertyChanged(); }
    }
    #endregion 属性end

    private void Execute(string obj)
    {
        switch (obj)
        {
            case "新增待办":
                AddToDo(null);
                break;
            case "新增备忘录":
                AddMemo(null);
                break;
        }
    }

    /// <summary>
    /// 添加待办事项
    /// </summary>
    private async void AddToDo(ToDoDto model)
    {
        DialogParameters param = new();
        if (model != null)
        {
            param.Add("Value", model);
        }

        var dialogResult = await dialog.ShowDialog("AddToDoView", null);
        if (dialogResult.Result == ButtonResult.OK)
        {
            try
            {
                UpdateLoading(true);

                var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (todo.Id > 0)
                {
                    var updateResult = await toDoService.UpdateAsync(todo);
                    if (updateResult.Status)
                    {
                        var todoModel = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(model.Id));
                        if (todoModel != null)
                        {
                            todoModel.Title = todo.Title;
                            todoModel.Content = todo.Content;
                        }
                    }
                }
                else
                {
                    var addResult = await toDoService.AddAsync(todo);
                    if (addResult.Status)
                    {
                        Summary.Sum += 1;
                        summary.ToDoList.Add(addResult.Result);
                        Summary.CompletedRatio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
                        this.Refresh();
                    }
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }
    }

    /// <summary>
    /// 添加备忘录
    /// </summary>
    private async void AddMemo(MemoDto model)
    {
        try
        {
            UpdateLoading(true);

            DialogParameters param = new();
            if (model != null)
            {
                param.Add("Value", model);
            }

            var dialogResult = await dialog.ShowDialog("AddMemoView", null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (memo.Id > 0)
                {
                    var updateResult = await memoService.UpdateAsync(memo);
                    if (updateResult.Status)
                    {
                        var memoModel = summary.MemoList.FirstOrDefault(t => t.Id.Equals(model.Id));
                        if (memoModel != null)
                        {
                            memoModel.Title = memo.Title;
                            memoModel.Content = memo.Content;
                        }
                    }
                }
                else
                {
                    var addResult = await memoService.AddAsync(memo);
                    if (addResult.Status)
                    {
                        summary.MemoList.Add(addResult.Result);
                    }
                }
            }
        }
        finally
        {
            UpdateLoading(false);
        }
    }

    /// <summary>
    /// 生成上方的任务栏
    /// </summary>
    void CreateTaskBars()
    {
        TaskBars = new();
        TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = nameof(ToDoView) });
        TaskBars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = nameof(ToDoView) });
        TaskBars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比率", Color = "#FF02C6DC", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = nameof(MemoView) });
    }

    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        var summaryResult = await toDoService.GetSummaryAsync();
        if (summaryResult.Status)
        {
            Summary = summaryResult.Result;
            Refresh();
        }
        base.OnNavigatedTo(navigationContext);
    }

    /// <summary>
    /// 刷新汇总数据
    /// </summary>
    private void Refresh()
    {
        TaskBars[0].Content = summary.Sum.ToString();
        TaskBars[1].Content = summary.CompletedCount.ToString();
        TaskBars[2].Content = summary.CompletedRatio;
        TaskBars[3].Content = summary.MemoCount.ToString();
    }
}