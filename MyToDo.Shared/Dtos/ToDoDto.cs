namespace MyToDo.Shared.Dtos;

public class ToDoDto : BaseDto
{
    private string title;

    public string Title
    {
        get { return title; }
        set { title = value; OnPropertyChanged(); }
    }

    private string content;

    public string Content
    {
        get { return content; }
        set { content = value; OnPropertyChanged(); }
    }

    private string status;

    public string Status
    {
        get { return status; }
        set { status = value; OnPropertyChanged(); }
    }

}