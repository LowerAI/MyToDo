using MaterialDesignThemes.Wpf;

using MyToDo.Extensions;

using Prism.Events;

using System.Windows;
using System.Windows.Input;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IEventAggregator aggregator)
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
            btnClose.Click += (sender, e) =>
            {
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
}
