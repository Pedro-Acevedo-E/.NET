using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LoginContacts.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LoginContactsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LoginContactsContext") ?? throw new InvalidOperationException("Connection string 'LoginContactsContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//added to access session
builder.Services.AddDistributedMemoryCache();

// Http Session options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
