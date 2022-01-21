using DryIoc;
using MyToDo.Common;
using MyToDo.Service;
using MyToDo.ViewModels;
using MyToDo.ViewModels.Dialogs;
using MyToDo.Views;
using MyToDo.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
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

    protected override void OnInitialized()
    {
        var dialog = Container.Resolve<IDialogService>();
        dialog.ShowDialog("LoginView", callback =>
         {
             if (callback.Result != ButtonResult.OK)
             {
                 Environment.Exit(0);
                 return;
             }

             var service = Current.MainWindow.DataContext as IConfigureProvider;
             if (service != null)
             {
                 service.Configure();
             }
             base.OnInitialized();
         });
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl")); // 注册Web服务
        containerRegistry.GetContainer().RegisterInstance("https://localhost:7295/", serviceKey: "webUrl"); // 关联Web服务与其Url

        containerRegistry.Register<ILoginService, LoginService>();
        containerRegistry.Register<IToDoService, ToDoService>();
        containerRegistry.Register<IMemoService, MemoService>();
        containerRegistry.Register<IDialogHostService, DialogHostService>();

        containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

        containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
        containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
        containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

        containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
        containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
        containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
        containerRegistry.RegisterForNavigation<AboutView>();
    }

    public static void Logout(IContainerProvider containerProvider)
    {
        Current.MainWindow.Hide();

        var dialog = containerProvider.Resolve<IDialogService>();
        dialog.ShowDialog("LoginView", callback =>
        {
            if (callback.Result != ButtonResult.OK)
            {
                Environment.Exit(0);
                return;
            }
            Current.MainWindow.Show();
        });
    }
}