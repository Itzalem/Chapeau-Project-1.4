using Chapeau_Project_1._4.Repositories;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Chapeau_Project_1._4.Repositories.OrderOverviewRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Services;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.OrderOverview;

namespace Chapeau_Project_1._4

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            
            builder.Services.AddSingleton<IMenuRepository, MenuRepository>();
            builder.Services.AddSingleton<IMenuService, MenuService>();

            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
            builder.Services.AddSingleton<IOrderService, OrderService>();

            builder.Services.AddSingleton<IOrderOverviewRepository, OrderOverviewRepository>();
            builder.Services.AddSingleton<IOrderOverviewService, OrderOverviewService>();

            builder.Services.AddSingleton<IPersonellRepository, PersonellRepository>();
            builder.Services.AddSingleton<IPersonellService, PersonellService>();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
