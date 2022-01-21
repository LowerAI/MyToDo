using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace MyToDo.ViewModels;

public class MainViewModel : BindableBase, IConfigureProvider
{
    /// <summary>
    /// 导航日志
    /// </summary>
    private IRegionNavigationJournal journal;
    private readonly IRegionManager regionManager;

    private ObservableCollection<MenuBar> menuBars;
    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<MenuBar> MenuBars
    {
        get { return menuBars; }
        set { menuBars = value; RaisePropertyChanged(); }
    }

    private string userName;
    /// <summary>
    /// 
    /// </summary>
    public string UserName
    {
        get { return userName; }
        set { userName = value; RaisePropertyChanged(); }
    }

    public DelegateCommand LogoutCommand { get; private set; }
    public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
    public DelegateCommand GoBackCommand { get; private set; }
    public DelegateCommand GoForwardCommand { get; private set; }

    public MainViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
    {
        MenuBars = new ObservableCollection<MenuBar>();
        LogoutCommand = new DelegateCommand(() =>
        {
            App.Logout(containerProvider);
        });
        NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
        this.regionManager = regionManager;

        GoBackCommand = new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoBack)
            {
                journal.GoBack();
            }
        });

        GoForwardCommand = new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoForward)
            {
                journal.GoForward();
            }
        });
    }

    private void Navigate(MenuBar obj)
    {
        if (obj == null || string.IsNullOrWhiteSpace(obj.Namespace))
        {
            return;
        }

        regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Namespace, back =>
        {
            journal = back.Context.NavigationService.Journal;
        });
    }

    void CreateMenuBar()
    {
        MenuBars.Add(new MenuBar { Icon = "Home", Title = "首页", Namespace = "IndexView" });
        MenuBars.Add(new MenuBar { Icon = "NotebookOutline", Title = "待办事项", Namespace = "ToDoView" });
        MenuBars.Add(new MenuBar { Icon = "NotebookPlus", Title = "备忘录", Namespace = "MemoView" });
        MenuBars.Add(new MenuBar { Icon = "Cog", Title = "设置", Namespace = "SettingsView" });
    }

    /// <summary>
    /// 配置首页初始化参数
    /// </summary>
    /// <param name="configuration"></param>
    public void Configure()
    {
        UserName = AppSession.UserName;
        CreateMenuBar();
        regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
    }
}