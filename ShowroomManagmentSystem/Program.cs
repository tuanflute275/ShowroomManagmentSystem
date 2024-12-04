using ShowroomManagmentSystem.Extensions;

var builder       = WebApplication.CreateBuilder(args);
var services      = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddDerivativeTradeServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.ConfigureMiddleware();

app.MapControllers();
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
