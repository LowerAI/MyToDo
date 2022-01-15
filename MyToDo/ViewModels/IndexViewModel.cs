using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using System.Collections.ObjectModel;

namespace MyToDo.ViewModels;

public class IndexViewModel : BindableBase
{
    public IndexViewModel(IDialogHostService dialog)
    {
        CreateTaskBars();
        
        ToDoDtos = new();
        MemoDtos = new();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        this.dialog = dialog;
    }

    #region    属性start
    private readonly IDialogHostService dialog;

    public DelegateCommand<string> ExecuteCommand { get; private set; }

    private ObservableCollection<TaskBar> taskBars;

    public ObservableCollection<TaskBar> TaskBars
    {
        get { return taskBars; }
        set { taskBars = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<ToDoDto> toDoDtos;

    public ObservableCollection<ToDoDto> ToDoDtos
    {
        get { return toDoDtos; }
        set { toDoDtos = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<MemoDto> memoDtos;

    public ObservableCollection<MemoDto> MemoDtos
    {
        get { return memoDtos; }
        set { memoDtos = value; RaisePropertyChanged(); }
    }
    #endregion 属性end

    private void Execute(string obj)
    {
        switch (obj)
        {
            case "新增待办":
                AddToDo();
                break;
            case "新增备忘录":
                AddMemo();
                break;
        }
    }

    private void AddToDo()
    {
        dialog.ShowDialog("AddToDoView", null);
    }

    private void AddMemo()
    {
        dialog.ShowDialog("AddMemoView", null);
    }

    /// <summary>
    /// 生成上方的任务栏
    /// </summary>
    void CreateTaskBars()
    {
        TaskBars = new();
        TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Content = "9", Color = "#FF1ECA3A", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比率", Content = "100%", Color = "#FF02C6DC", Target = "" });
        TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Content = "19", Color = "#FFFFA000", Target = "" });
    }
}