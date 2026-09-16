using System.Text.Json;
using EnglandTechnologies.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowPortfolio");
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "docs",
        pattern: "docs/{*slug}",
        defaults: new { controller = "Docs", action = "Page" })
    .WithStaticAssets();

app.MapHub<EnglandTechnologies.Hubs.MetricsHub>("/metricsHub");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();