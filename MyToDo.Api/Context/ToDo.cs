namespace MyToDo.Api.Context;

public class ToDo : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
}