using AutoMapper;

using Microsoft.EntityFrameworkCore;

using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Api.Extensions;
using MyToDo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region    ע�����ݿ���������Ķ�������ʵ�������
var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
builder.Services.AddDbContext<MyToDoContext>(option => option.UseSqlite(connectionString))
    .AddUnitOfWork<MyToDoContext>()
    .AddCustomRepository<ToDo, ToDoRepository>()
    .AddCustomRepository<Memo, MemoRepository>()
    .AddCustomRepository<User, UserRepository>();

builder.Services.AddTransient<IToDoService, ToDoService>();
builder.Services.AddTransient<IMemoService, MemoService>();
builder.Services.AddTransient<ILoginService, LoginService>();
#endregion

var autoMapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperProFile());
});
builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

builder.Services.AddControllers(); // ����ȷ������Swagger�п���Controller
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting(); // ������app.UseEndpoints...�����Ⱦ�����
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

//app.MapMethods("/ToDo", new[] { "GET", "POST", "PUT", "PATCH", "DELETE" }, (HttpRequest req) => $"��ǰHttp������{req.Method}");

// ����ȷ����·�ɵ���ȷ��Controller
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();