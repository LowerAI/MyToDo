using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace MyToDo.ViewModels.Dialogs;

public class LoginViewModel : BindableBase, IDialogAware
{
    public string Title { get; set; } = "ToDo";

    private string account;
    /// <summary>
    /// 帐号
    /// </summary>
    public string Account
    {
        get { return account; }
        set { account = value; RaisePropertyChanged(); }
    }

    private string password;
    private readonly ILoginService _loginService;
    private readonly IEventAggregator _aggregator;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password
    {
        get { return password; }
        set { password = value; RaisePropertyChanged(); }
    }

    private int selectedIndex;
    /// <summary>
    /// 
    /// </summary>
    public int SelectedIndex
    {
        get { return selectedIndex; }
        set { selectedIndex = value; RaisePropertyChanged(); }
    }

    private RegisterUserDto userDto;

    public RegisterUserDto UserDto
    {
        get { return userDto; }
        set { userDto = value; RaisePropertyChanged(); }
    }

    public DelegateCommand<string> ExecuteCommand { get; private set; }

    public event Action<IDialogResult> RequestClose;

    public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
    {
        UserDto = new RegisterUserDto();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        _loginService = loginService;
        _aggregator = aggregator;
    }

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        LoginOut();
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {

    }

    private void Execute(string arg)
    {
        switch (arg)
        {
            case "Go":      // 跳转注册页面
                Go();
                break;
            case "Login":
                Login();
                break;
            case "LoginOut":
                LoginOut();
                break;
            case "Register": // 注册帐号
                Register();
                break;
            case "Return":   // 返回登录页面
                Return();
                break;
        }
    }

    private void Go()
    {
        SelectedIndex = 1;
    }

    private async void Login()
    {
        if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        var loginResult = await _loginService.LoginAsync(new UserDto
        {
            Account = Account,
            Password = Password.GetMD5()
        });

        if (loginResult.Status)
        {
            AppSession.UserName = loginResult.Result.UserName;
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            return;
        }

        _aggregator.SendMessage(loginResult.Message,"Login");
    }

    private void LoginOut()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
    }

    private async void Register()
    {
        if (string.IsNullOrWhiteSpace(UserDto.Account) || string.IsNullOrWhiteSpace(UserDto.UserName) || string.IsNullOrWhiteSpace(UserDto.Password) || string.IsNullOrWhiteSpace(UserDto.NewPassword))
        {
            return;
        }

        if (!UserDto.Password.Equals(UserDto.NewPassword))
        {
            //验证失败提示。。
            _aggregator.SendMessage("两次的密码不一致，请检查！", "Login");
            return;
        }

        var registerResult = await _loginService.RegisterAsync(new UserDto
        {
            Account = UserDto.Account,
            UserName = UserDto.UserName,
            Password = UserDto.Password
        });

        if (registerResult.Status)
        {// 注册成功
            _aggregator.SendMessage("注册成功", "Login");
            SelectedIndex = 0;
            return;
        }

        _aggregator.SendMessage(registerResult.Message, "Login");
    }

    private void Return()
    {
        SelectedIndex = 0;
    }
}