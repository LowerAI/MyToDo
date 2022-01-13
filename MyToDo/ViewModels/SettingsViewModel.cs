using MyToDo.Common.Models;
using MyToDo.Extensions;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using System.Collections.ObjectModel;

namespace MyToDo.ViewModels;

public class SettingsViewModel : BindableBase
{
    public SettingsViewModel(IRegionManager regionManager)
    {
        this.regionManager = regionManager;
        menuBars = new ObservableCollection<MenuBar>();
        CreateMenuBar();
        NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
    }

    private void Navigate(MenuBar obj)
    {
        if (obj == null || string.IsNullOrWhiteSpace(obj.Namespace))
        {
            return;
        }

        regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.Namespace);
    }

    public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
    private ObservableCollection<MenuBar> menuBars;
    private readonly IRegionManager regionManager;

    public ObservableCollection<MenuBar> MenuBars
    {
        get { return menuBars; }
        set { menuBars = value; RaisePropertyChanged(); }
    }

    void CreateMenuBar()
    {
        MenuBars.Add(new MenuBar { Icon = "Pelette", Title = "个性化", Namespace = "SkinView" });
        MenuBars.Add(new MenuBar { Icon = "Cog", Title = "系统设置", Namespace = "" });
        MenuBars.Add(new MenuBar { Icon = "Information", Title = "关于更多", Namespace = "AboutView" });
    }
}