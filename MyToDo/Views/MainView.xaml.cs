using MaterialDesignThemes.Wpf;

using MyToDo.Common;
using MyToDo.Extensions;

using Prism.Events;

using System.Windows;
using System.Windows.Input;

namespace MyToDo.Views;

/// <summary>
/// MainView.xaml 的交互逻辑
/// </summary>
public partial class MainView : Window
{
    public MainView(IEventAggregator aggregator, IDialogHostService dialogHost)
    {
        InitializeComponent();

        // 注册等待消息窗口
        aggregator.Register(arg =>
        {
            DialogHost.IsOpen = arg.IsOpen;

            if (DialogHost.IsOpen)
            {
                DialogHost.DialogContent = new ProgressView();
            }
        });

        btnMin.Click += (sender, e) =>
        {
            this.WindowState = WindowState.Minimized;
        };
        btnMax.Click += (sender, e) =>
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        };
        btnClose.Click += async (sender, e) =>
        {
            var dialogResult = await dialogHost.Question("温馨提示", "确认退出系统？");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
            {
                return;
            }

            this.Close();
        };
        ColorZone.MouseMove += (sender, e) =>
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        };
        ColorZone.MouseDoubleClick += (sender, e) =>
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        };

        menuBar.SelectionChanged += (sender, e) =>
        {
            drawerHost.IsLeftDrawerOpen = false;
        };
    }
}