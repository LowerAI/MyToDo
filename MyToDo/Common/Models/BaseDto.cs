using System;

namespace MyToDo.Common.Models;

public class BaseDto
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }
}