using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using kütüphaneuygulaması.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<kütüphaneuygulamasıContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("kütüphaneuygulamasıContext") ?? throw new InvalidOperationException("Connection string 'kütüphaneuygulamasıContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // API controller desteği
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });

var app = builder.Build();

// Seed işlemi
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<kütüphaneuygulamasıContext>();
    DbInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=usersaccounts}/{action=login}/{id?}");

app.MapControllers(); // API controller'ları için

app.Run();
