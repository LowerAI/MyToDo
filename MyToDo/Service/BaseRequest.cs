using RestSharp;

namespace MyToDo.Service;

public class BaseRequest
{
    public Method Method { get; set; }
    public string Route { get; set; }
    public string ContentType { get; set; } = "application/json";
    public PlaceToAddParameter Place { get; set; } = PlaceToAddParameter.None;
    public object Parameter { get; set; }
}

/// <summary>
/// BaseRequest的参数位置
/// </summary>
public enum PlaceToAddParameter
{
    /// <summary>
    /// 添加到RequestBody，默认序列化为json格式
    /// </summary>
    Body,
    /// <summary>
    /// 添加到RequestBody的Form中，键值对格式
    /// </summary>
    Form,
    /// <summary>
    /// 添加到RequestHeader中
    /// </summary>
    Header,
    /// <summary>
    /// 无参数时的默认值
    /// </summary>
    None,
    /// <summary>
    /// 添加到Url中?后面组成QueryString
    /// </summary>
    Query,
    /// <summary>
    /// 添加到Url的最后的/与?之前的分段，组成完整的路由字符串
    /// </summary>
    UrlSegment
}