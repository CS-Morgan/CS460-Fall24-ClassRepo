using AuctionHouse2.Models;
using Microsoft.EntityFrameworkCore;
using AuctionHouse2.DAL.Abstract;
using AuctionHouse2.DAL.Concrete;

namespace AuctionHouse2;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AuctionHouseDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("AuctionHouseConnection")));
        builder.Services.AddScoped<DbContext, AuctionHouseDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}