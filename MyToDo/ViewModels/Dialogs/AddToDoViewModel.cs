using MaterialDesignThemes.Wpf;

using MyToDo.Common;

using Prism.Commands;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs;

public class AddToDoViewModel : IDialogHostAware
{
    public AddToDoViewModel()
    {
        SaveCommand = new DelegateCommand(Save);
        CancelCommand = new DelegateCommand(Cancel);
    }

    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            DialogHost.Close(DialogHostName);
        }
    }

    private void Save()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            var param = new DialogParameters();
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
        }
    }

    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }
}