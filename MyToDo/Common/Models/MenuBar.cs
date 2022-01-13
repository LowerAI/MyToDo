using Prism.Mvvm;

namespace MyToDo.Common.Models;

/// <summary>
/// 系统导航菜单实体类
/// </summary>
public class MenuBar : BindableBase
{
    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 菜单命名空间
    /// </summary>
    public string Namespace { get; set; }
}