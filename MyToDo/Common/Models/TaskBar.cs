using Prism.Mvvm;

namespace MyToDo.Common.Models;

/// <summary>
/// 任务类
/// </summary>
public class TaskBar : BindableBase
{
    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    private string content;
    /// <summary>
    /// 内容
    /// </summary>
    public string Content
    {
        get { return content; }
        set { content = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color { get; set; }
    /// <summary>
    /// 目标
    /// </summary>
    public string Target { get; set; }
}