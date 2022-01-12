using Microsoft.AspNetCore.Components.Routing;

using MyToDo.Service;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyToDo.ViewModels;

public class ToDoViewModel : NavigateViewModel
{
    public ToDoViewModel(IToDoService service, IContainerProvider provider) : base(provider)
    {
        _service = service;
        ToDoDtos = new();
        AddCommand = new DelegateCommand(Add);
    }

    private void Add()
    {
        IsRightDrawerOpen = true;
    }

    private bool isRightDrawerOpen;
    /// <summary>
    /// 右侧添加窗口是否展开
    /// </summary>
    public bool IsRightDrawerOpen
    {
        get { return isRightDrawerOpen; }
        set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }


    public DelegateCommand AddCommand { get; private set; }

    private ObservableCollection<ToDoDto> toDoDtos;
    private readonly IToDoService _service;

    public ObservableCollection<ToDoDto> ToDoDtos
    {
        get { return toDoDtos; }
        set { toDoDtos = value; RaisePropertyChanged(); }
    }

    async void GetDataAsync()
    {
        UpdateLoading(true);

        var todoResult = await _service.GetAllAsync(new Shared.Parameters.QueryParameters()
        {
            PageIndex = 0,
            PageSize = 100
        });

        if (todoResult.Status)
        {
            ToDoDtos.Clear();
            foreach (var item in todoResult.Result.Items)
            {
                ToDoDtos.Add(item);
            }
        }

        UpdateLoading(false);
    }

    public override void OnNavigatedTo(Prism.Regions.NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        GetDataAsync();
    }
}
