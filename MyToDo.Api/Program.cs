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

#region    注入数据库操作上下文对象和相关实体与服务
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

builder.Services.AddControllers(); // 本句确保能在Swagger中看到Controller
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting(); // 本句是app.UseEndpoints...这句的先决条件
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

//app.MapMethods("/ToDo", new[] { "GET", "POST", "PUT", "PATCH", "DELETE" }, (HttpRequest req) => $"当前Http方法是{req.Method}");

// 本句确保能路由到正确的Controller
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();