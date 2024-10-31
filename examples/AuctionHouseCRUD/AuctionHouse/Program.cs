using AuctionHouse.DAL.Abstract;
using AuctionHouse.DAL.Concrete;
using AuctionHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        // Add our dbcontext to the Dependency Injection container
        builder.Services.AddDbContext<AuctionHouseDbContext>(
                options => options
                .UseLazyLoadingProxies()    
                .UseSqlServer(
                    builder.Configuration.GetConnectionString("AuctionHouseConnection")));
        builder.Services.AddScoped<DbContext,AuctionHouseDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
        builder.Services.AddScoped<IItemRepository, ItemRepository>();
        builder.Services.AddScoped<ISellerRepository, SellerRepository>();

        // Add Swagger for REST API live documentation and testing
        // Needs packages Swashbuckle.AspNetCore.SwaggerGen and Swashbuckle.AspNetCore.SwaggerUI
        builder.Services.AddSwaggerGen();


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
            app.UseSwagger();
            app.UseSwaggerUI();
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
