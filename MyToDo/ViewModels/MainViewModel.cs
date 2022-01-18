using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using System.Collections.ObjectModel;

namespace MyToDo.ViewModels;

public class MainViewModel : BindableBase, IConfigureProvider
{
    public MainViewModel(IRegionManager regionManager)
    {
        MenuBars = new ObservableCollection<MenuBar>();
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

    public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
    public DelegateCommand GoBackCommand { get; private set; }
    public DelegateCommand GoForwardCommand { get; private set; }

    private ObservableCollection<MenuBar> menuBars;
    private readonly IRegionManager regionManager;

    /// <summary>
    /// 导航日志
    /// </summary>
    private IRegionNavigationJournal journal;

    public ObservableCollection<MenuBar> MenuBars
    {
        get { return menuBars; }
        set { menuBars = value; RaisePropertyChanged(); }
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
        CreateMenuBar();

        regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
    }
}