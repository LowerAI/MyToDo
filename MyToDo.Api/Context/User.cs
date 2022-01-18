namespace MyToDo.Api.Context;

/// <summary>
/// 用户实体类
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// 帐号
    /// </summary>
    public string Account { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}