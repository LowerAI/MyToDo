using MaterialDesignThemes.Wpf;

using MyToDo.Common;
using MyToDo.Shared.Dtos;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs;

public class AddToDoViewModel : BindableBase, IDialogHostAware
{
    public AddToDoViewModel()
    {
        SaveCommand = new DelegateCommand(Save);
        CancelCommand = new DelegateCommand(Cancel);
    }

    private ToDoDto model;

    public ToDoDto Model
    {
        get { return model; }
        set { model = value; RaisePropertyChanged(); }
    }


    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
        }
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Model.Title) || string.IsNullOrWhiteSpace(Model.Content))
        {
            return;
        }

        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            var param = new DialogParameters();
            param.Add("Value", Model);
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
        }
    }

    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        if (parameters.ContainsKey("Value"))
        {
            Model = parameters.GetValue<ToDoDto>("Value");
        }
        else
        {
            Model = new ToDoDto();
        }
    }
}