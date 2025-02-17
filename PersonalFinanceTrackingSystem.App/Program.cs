using PersonalFinanceTrackingSystem.App.Components;
using System;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Register;
using Microsoft.AspNetCore.Components.Authorization;
using PersonalFinanceTrackingSystem.App.Service;
using PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;
using PersonalFinanceTrackingSystem.App.Service.Security;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region DbService

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(connectionString);
},
ServiceLifetime.Transient,
ServiceLifetime.Transient);

#endregion

builder.Services.AddScoped<IInjectService, InjectService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<BudgetSetupService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
