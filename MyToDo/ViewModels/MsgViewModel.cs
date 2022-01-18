using MaterialDesignThemes.Wpf;

using MyToDo.Common;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels;

public class MsgViewModel : BindableBase, IDialogHostAware
{
    public MsgViewModel()
    {
        SaveCommand = new DelegateCommand(Save);
        CancelCommand = new DelegateCommand(Cancel);
    }

    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public string DialogHostName { get; set; }

    private string title;

    public string Title
    {
        get { return title; }
        set { title = value; RaisePropertyChanged(); }
    }

    private string content;

    public string Content
    {
        get { return content; }
        set { content = value; RaisePropertyChanged(); }
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        if (parameters.ContainsKey(nameof(Title)))
        {
            Title = parameters.GetValue<string>(nameof(Title)); 
        }
        if (parameters.ContainsKey(nameof(Content)))
        {
            Content = parameters.GetValue<string>(nameof(Content)); 
        }
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
        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            var param = new DialogParameters();
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
        }
    }
}