using Autofac.Extensions.DependencyInjection; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Logging; 
using System; 
using System.IO; 
using log4net; 
using log4net.Config; 
using eShopLegacyMVC_Core.Services;
var builder = WebApplication.CreateBuilder(args); 
// Configure logging 
builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddLog4Net("log4net.config"); 
// Add services to the container. 
builder.Services.AddControllersWithViews(); 
builder.Services.AddScoped<ICatalogService, CatalogService>(); // Scoped lifetime is commonly used for services that interact with the DB
// Configure Autofac 
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); 
// Load configuration 
var configuration = builder.Configuration; 
var environment = builder.Environment; 
// Log the environment for debugging purposes 
Console.WriteLine($"Environment: {environment.EnvironmentName}"); 
// Bind to 0.0.0.0 to allow external access
builder.WebHost.UseUrls("http://0.0.0.0:5001");
var app = builder.Build(); 
// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment()) 
{ 
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts(); 
} 
//app.UseHttpsRedirection(); 
app.UseStaticFiles(); 
app.UseRouting(); 
app.UseAuthorization(); 
app.MapControllerRoute( 
    name: "default", 
    pattern: "{controller=Catalog}/{action=Index}/{id?}"); 
app.Run();
