using System.Text.Json;
using EnglandTechnologies.Services;

var builder = WebApplication.CreateBuilder(args);

// Hosts that belong to the main site
var mainHosts = new[]
{
    "englandtechnologies.net",
    "www.englandtechnologies.net",
    "portfolio.englandtechnologies.net", // only if this is served by this app
    "localhost"
};

// Hosts that belong to the tech support site
var supportHosts = new[]
{
    "techsupport.englandtechnologies.net",
    "techsupport.localhost"
};

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MetricsStore>();
builder.Services.AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPortfolio", policy =>
    {
        policy.WithOrigins(
                "https://englandtechnologies.net",
                "https://portfolio.englandtechnologies.net"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Rejects any Host header not listed in AllowedHosts (appsettings.json)
app.UseHostFiltering();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowPortfolio");
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "docs",
        pattern: "docs/{*slug}",
        defaults: new { controller = "Docs", action = "Page" })
    .RequireHost(mainHosts)
    .WithStaticAssets();

// Tech support subdomain (also reachable locally at http://techsupport.localhost:5255).
// Must be mapped before the catch-all "default" route so the host constraint wins.
app.MapControllerRoute(
        name: "techsupport",
        pattern: "{action=Index}/{id?}",
        defaults: new { controller = "TechSupport" })
    .RequireHost(supportHosts)
    .WithStaticAssets();

app.MapHub<EnglandTechnologies.Hubs.MetricsHub>("/metricsHub")
    .RequireHost(mainHosts);

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireHost(mainHosts)
    .WithStaticAssets();

app.Run();