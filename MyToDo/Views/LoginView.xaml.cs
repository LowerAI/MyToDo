using MyToDo.Extensions;
using Prism.Events;
using System.Windows.Controls;

namespace MyToDo.Views;

/// <summary>
/// LoginView.xaml 的交互逻辑
/// </summary>
public partial class LoginView : UserControl
{
    public LoginView(IEventAggregator agregator)
    {
        InitializeComponent();

        agregator.RegisterMessage(arg =>
        {
            LoginSnackBar.MessageQueue?.Enqueue(arg.Message);
        }, "Login");
    }
}