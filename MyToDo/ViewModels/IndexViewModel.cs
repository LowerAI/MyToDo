using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels;

public class IndexViewModel : NavigationViewModel
{
    private readonly IToDoService toDoService;
    private readonly IMemoService memoService;
    private readonly IDialogHostService dialog;

    public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
    {
        CreateTaskBars();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
        ToDoCompletedCommand = new DelegateCommand<ToDoDto>(Completed);
        EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
        toDoService = provider.Resolve<IToDoService>();

        this.dialog = dialog;
    }

    private async void Completed(ToDoDto obj)
    {
        var updateResult = await toDoService.UpdateAsync(obj);
        if (updateResult.Status)
        {
            var todo = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
            if (todo != null)
            {
                summary.ToDoList.Remove(todo);
            }
        }
    }

    #region    属性start
    public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
    public DelegateCommand<ToDoDto> ToDoCompletedCommand { get; private set; }
    public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
    public DelegateCommand<string> ExecuteCommand { get; private set; }

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
        if (dialogResult.Result == Prism.Services.Dialogs.ButtonResult.OK)
        {
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
                    summary.ToDoList.Add(addResult.Result);
                }
            }
        }
    }

    /// <summary>
    /// 添加备忘录
    /// </summary>
    private async void AddMemo(MemoDto model)
    {
        DialogParameters param = new();
        if (model != null)
        {
            param.Add("Value", model);
        }

        var dialogResult = await dialog.ShowDialog("AddMemoView", null);
        if (dialogResult.Result == Prism.Services.Dialogs.ButtonResult.OK)
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

    /// <summary>
    /// 生成上方的任务栏
    /// </summary>
    void CreateTaskBars()
    {
        TaskBars = new();
        TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比率", Color = "#FF02C6DC", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "" });
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