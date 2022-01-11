using MyToDo.Common.Models;
using MyToDo.Service;

using Prism.Commands;
using Prism.Mvvm;

using System.Collections.ObjectModel;

namespace MyToDo.ViewModels;

public class ToDoViewModel : BindableBase
{
    public ToDoViewModel(IToDoService service)
    {
        _service = service;
        ToDoDtos = new();
        AddCommand = new DelegateCommand(Add);
        CreateToDoList();
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

    async void CreateToDoList()
    {
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
    }
}
