namespace MyToDo.Api.Context;

/// <summary>
/// 待办事项实体类
/// </summary>
public class ToDo : BaseEntity
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }
}