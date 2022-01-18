namespace MyToDo.Api.Context;

/// <summary>
/// 备忘录实体类
/// </summary>
public class Memo : BaseEntity
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}