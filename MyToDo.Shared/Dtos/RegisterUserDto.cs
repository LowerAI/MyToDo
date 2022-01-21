namespace MyToDo.Shared.Dtos;

/// <summary>
/// 注册专用的UserDto
/// </summary>
public class RegisterUserDto : UserDto
{
    private string newPassword;

    public string NewPassword
    {
        get { return newPassword; }
        set { newPassword = value; }
    }

}