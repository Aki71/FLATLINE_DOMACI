using BethanysPieShopAdmin.Data;
using BethanysPieShopAdmin.Data.Models.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TreningDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("BethanysPieShopDbContextConnection")));

builder.Services.AddScoped<ITreningRepository, TreningRepository>();
builder.Services.AddScoped<IVezbaRepository, VezbaRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<TreningDbContext>();
    DbInitializerTrening.Seed(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
