using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using dotnet_Api.Data;
using dotnet_Api.Installer;
using dotnet_Api.Interfaces;
using dotnet_Api.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServiceInAssembly(builder.Configuration);

/*
ถ้าต้องการเพิ่ม builder.Services.MyMethod เราต้อง ทำ extensions C#  เพิ่มทำให้้ Serivces มี meathoad นั้น
*/

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Automatic add services
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()) //Assembly (System.Reflection)
    .Where(t => t.Name.EndsWith("Service"))
    .AsImplementedInterfaces();
});

//builder.Services.AddScoped<IAccountService, AccountService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
//middleware (development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_Api"));
}

// middleware
app.UseStaticFiles();

app.UseHttpsRedirection(); // auto http to https

app.UseCors("AllowSpeificOrigins");

app.UseAuthentication();

app.UseAuthorization(); // ตัวสอบสิทธิ

app.MapControllers();

app.Run();
