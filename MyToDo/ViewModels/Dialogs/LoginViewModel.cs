using Prism.Commands;
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
    /// <summary>
    /// 密码
    /// </summary>
    public string Password
    {
        get { return password; }
        set { password = value; RaisePropertyChanged(); }
    }

    public DelegateCommand<string> ExecutedCommand { get; private set; }

    public event Action<IDialogResult> RequestClose;

    public LoginViewModel()
    {
        ExecutedCommand = new DelegateCommand<string>(Execute);
    }

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    private void Execute(string arg)
    {
        switch (arg)
        {
            case "Login":
                Login();
                break;
            case "LoginOut":
                LoginOut();
                break;
        }
    }

    private void LoginOut()
    {
    }

    private void Login()
    {
    }
}