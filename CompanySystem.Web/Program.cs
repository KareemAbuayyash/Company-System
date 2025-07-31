using CompanySystem.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCompanySystemServices(builder.Configuration);
builder.Services.AddCompanySystemMvc();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
await app.UseCompanySystemAsync(builder.Environment);

// Configure routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Custom fallback - redirect to home page
app.MapFallback(async (context) =>
{
    context.Response.Redirect("/Home/Index");
});

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
