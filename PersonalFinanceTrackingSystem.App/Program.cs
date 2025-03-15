using PersonalFinanceTrackingSystem.App.Components;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackingSystem.Database.EfAppDbContextModels;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Login;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Register;
using Microsoft.AspNetCore.Components.Authorization;
using PersonalFinanceTrackingSystem.App.Service;
using PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup;
using PersonalFinanceTrackingSystem.App.Service.Security;
using MudBlazor.Services;
using PersonalFinanceTrackingSystem.Domain.Features.Authentication.Profile;
using PersonalFinanceTrackingSystem.Shared.DapperService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Load Serilog configuration from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add Serilog to logging
builder.Host.UseSerilog();

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
builder.Services.AddScoped<DapperService>(x => new DapperService(connectionString));

#endregion

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IInjectService, InjectService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<BudgetSetupService>();
builder.Services.AddScoped<TransactionTrackingService>();
builder.Services.AddScoped<ProfileService>();


var app = builder.Build();

// Middleware to log requests
app.UseSerilogRequestLogging();

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
