using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using MyToDo.Api;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Api.Extensions;
using MyToDo.Api.Services;

using System.Reflection;

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

// 本句确保能在Swagger中看到Controller
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // 获取xml注释文件路径
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName), true); // 加载注释文件并显示其中的注释

    //options.SwaggerDoc("V1", new OpenApiInfo
    //{
    //    Title = "MyToDo",
    //    Version = "V1",
    //    Description = "MyToDo:V1版"
    //});

    typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
{// Swagger版本配置
    options.SwaggerDoc(version, new OpenApiInfo
    {
        Title = "MyToDo",
        Version = version,
        Description = $"MyToDo:{version}版"
    });
});
});

var app = builder.Build();
app.UseRouting(); // 本句是app.UseEndpoints...这句的先决条件
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // 如果只有一个版本也要和上方保持一致
    app.UseSwaggerUI(options =>
    {
        typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
        {
            // 切换版本操作
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"版本选择：{version}");
        });
    });
}


app.UseHttpsRedirection();

//app.MapMethods("/ToDo", new[] { "GET", "POST", "PUT", "PATCH", "DELETE" }, (HttpRequest req) => $"当前Http方法是{req.Method}");

// 本句确保能路由到正确的Controller
app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

/// <summary>
/// 查询概览信息
/// </summary>
app.MapGet("/api/Summarys", async Task<IResult> (IToDoService _service) =>
{
    var result = await _service.GetSummaryAsync();
    if (result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

app.Run();