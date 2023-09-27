using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChatSharp;
using ChatSharp.Core.Data;
using ChatSharp.Core.Starter;
using ChatSharp.Engine;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("connection.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration["ConnectionString"]; 

builder.Services.AddDbContext<ChatSharpDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureLogging(SetupLogging);

// Add other services
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddRazorPages();

// Add services to the Autofac container.
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.ConfigureContainer();
});

builder.Services.ConfigureSettings();

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// add env static helper
var programHelper = new ProgramEngineHelper(isDevelopment: app.Environment.IsDevelopment());
programHelper.Initialize();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.MapHub<ChatSharpHub>("/chatSharpHub");

app.Run();


void SetupLogging(ILoggingBuilder loggingBuilder)
{
    loggingBuilder.ClearProviders();
}
