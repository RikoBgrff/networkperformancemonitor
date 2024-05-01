using Microsoft.EntityFrameworkCore;
using networkperformancemonitor.BLL.Interfaces;
using networkperformancemonitor.BLL.Managers;
using networkperformancemonitor.DAL.Context;
using networkperformancemonitor.DAL.Interface;
using networkperformancemonitor.DAL.Repository;
using networkperformancemonitor.DAL.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<INetworkService, NetworkManager>();
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseSqlServer(builder
    .Configuration
    .GetConnectionString("npmonitor")));
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PerformanceMonitor}/{action=Dashboard}/{id?}");
app.Run();
