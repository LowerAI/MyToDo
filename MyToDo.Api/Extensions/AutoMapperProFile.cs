using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Extensions;

public class AutoMapperProFile : MapperConfigurationExpression
{
    public AutoMapperProFile()
    {
        CreateMap<ToDo, ToDoDto>().ReverseMap();
        CreateMap<Memo, MemoDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<PagedList<ToDo>, PagedList<ToDoDto>>();
        CreateMap<PagedList<Memo>, PagedList<MemoDto>>();
        CreateMap<PagedList<User>, PagedList<UserDto>>();
    }
}