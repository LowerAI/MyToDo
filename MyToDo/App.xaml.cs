using DryIoc;

using MyToDo.Service;
using MyToDo.ViewModels;
using MyToDo.Views;

using Prism.DryIoc;
using Prism.Ioc;

using System.Windows;

namespace MyToDo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainView>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl")); // 注册Web服务
        containerRegistry.GetContainer().RegisterInstance("https://localhost:7295/", serviceKey: "webUrl"); // 关联Web服务与其Url

        containerRegistry.Register<IToDoService, ToDoService>();
        containerRegistry.Register<IMemoService, MemoService>();

        containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
        containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
        containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
        containerRegistry.RegisterForNavigation<AboutView>();
    }
}