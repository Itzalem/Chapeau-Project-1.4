using Chapeau_Project_1._4.Repositories.BillRepo;
using Chapeau_Project_1._4.Repositories.DrinkRepo;

//using Chapeau_Project_1._4.Repositories.DrinkRepo;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Repositories.PersonellRepo;
using Chapeau_Project_1._4.Repositories.RestaurantTableRepo;
using Chapeau_Project_1._4.Services;
using Chapeau_Project_1._4.Services.Bill;
//using Chapeau_Project_1._4.Services.Drinks;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.RestaurantTableService;

namespace Chapeau_Project_1._4

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add session support
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // adjust as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //builder.Services.AddControllersWithViews();

            //Added by Lukas
            builder.Services.AddSingleton<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddSingleton<IOrderItemService, OrderItemService>();

            builder.Services.AddSingleton<IOrderService, OrderService>();
            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

            builder.Services.AddSingleton<IMenuRepository, MenuRepository>();
            builder.Services.AddSingleton<IMenuService, MenuService>();

            builder.Services.AddSingleton<IOrderItemService, OrderItemService>();
            builder.Services.AddSingleton<IOrderItemRepository, OrderItemRepository>();

            builder.Services.AddSingleton<IPersonellRepository, PersonellRepository>();
            builder.Services.AddSingleton<IPersonellService, PersonellService>();

            builder.Services.AddSingleton<IRestaurantTableService, RestaurantTableService>();
            builder.Services.AddSingleton<IRestaurantTableRepository, RestaurantTableRepository>();

            //builder.Services.AddSingleton<IDrinkService, DrinkService>();
            builder.Services.AddSingleton<IDrinkRepository,DrinkRepository>();

            builder.Services.AddSingleton<IBillService, BillService>();
            builder.Services.AddSingleton<IBillRepository, BillRepository>();

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

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
