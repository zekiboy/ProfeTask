using System.Data;
using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using profe.webui.Bases;

using profe.webui.Data.Context;
using profe.webui.Data.EmailService;
using profe.webui.Data.Repositories;
using profe.webui.Entities;
using profe.webui;

using static System.Runtime.InteropServices.JavaScript.JSType;
using profe.webui.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using profe.webui.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddServiceRegistration(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

 var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add Exceptions from Application Layer
//app.ConfigureExceptionHandlingMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();

app.ConfigureExceptionHandlingMiddleware();

//app.UseMiddleware<ExceptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



