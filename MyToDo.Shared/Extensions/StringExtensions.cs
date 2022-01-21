using System;
using System.Security.Cryptography;
using System.Text;

namespace MyToDo.Shared.Extensions;

/// <summary>
/// 字符串类型的扩展
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 生成md5
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetMD5(this string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            throw new ArgumentNullException(nameof(data));
        }

        var hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}