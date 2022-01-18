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

// ����ȷ������Swagger�п���Controller
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // ��ȡxmlע���ļ�·��
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName), true); // ����ע���ļ�����ʾ���е�ע��

    //options.SwaggerDoc("V1", new OpenApiInfo
    //{
    //    Title = "MyToDo",
    //    Version = "V1",
    //    Description = "MyToDo:V1��"
    //});

    typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
{// Swagger�汾����
    options.SwaggerDoc(version, new OpenApiInfo
    {
        Title = "MyToDo",
        Version = version,
        Description = $"MyToDo:{version}��"
    });
});
});

var app = builder.Build();
app.UseRouting(); // ������app.UseEndpoints...�����Ⱦ�����
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // ���ֻ��һ���汾ҲҪ���Ϸ�����һ��
    app.UseSwaggerUI(options =>
    {
        typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
        {
            // �л��汾����
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"�汾ѡ��{version}");
        });
    });
}


app.UseHttpsRedirection();

//app.MapMethods("/ToDo", new[] { "GET", "POST", "PUT", "PATCH", "DELETE" }, (HttpRequest req) => $"��ǰHttp������{req.Method}");

// ����ȷ����·�ɵ���ȷ��Controller
app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

/// <summary>
/// ��ѯ������Ϣ
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