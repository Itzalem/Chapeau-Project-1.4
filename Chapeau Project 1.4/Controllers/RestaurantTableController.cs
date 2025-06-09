using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.RestaurantTableService;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

public class RestaurantTableController : Controller
{
    private readonly IRestaurantTableService _tableService;
    private readonly IOrderService _orderService;

    public RestaurantTableController(IRestaurantTableService tableService, IOrderService orderService)
    {
        _tableService = tableService;
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult Overview()
    {
        var viewModels = _tableService.GetTablesWithOrderStatus();
        return View(viewModels);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var table = _tableService.GetAllTables().FirstOrDefault(t => t.TableNumber == id);
        if (table == null)
            return NotFound();

        var order = _orderService.GetOrderByTable(id);
        bool canBeFreed = !table.IsOccupied || order == null || order.Status == EOrderStatus.paid;
        ViewBag.CanToggle = canBeFreed;
        ViewBag.TableNumber = id;

        if (order != null)
        {
            order.OrderItems = _orderService.GetOrderItems(order.OrderNumber);
            ViewBag.Order = order;
        }

        return View(table);
    }

    [HttpPost]
    public IActionResult ToggleOccupancy(int id)
    {
        var table = _tableService.GetAllTables().FirstOrDefault(t => t.TableNumber == id);
        if (table == null)
            return NotFound();

        var order = _orderService.GetOrderByTable(id);
        if (table.IsOccupied && order != null && order.Status != EOrderStatus.paid)
        {
            TempData["ToggleError"] = "Table has unpaid or active orders and cannot be marked free.";
            return RedirectToAction("Details", new { id });
        }

        bool newStatus = !table.IsOccupied;

        _tableService.SetManualFreed(id, !newStatus); //if we're freeing it, set flag to true
        _tableService.UpdateTableOccupancy(id, newStatus);

        return RedirectToAction("Overview");
    }

    [HttpPost]
    public IActionResult ServeFood(int id)
    {
        var order = _orderService.GetOrderByTable(id);
        if (order != null)
        {
            _orderService.ServeFoodItems(order.OrderNumber);
        }
        return RedirectToAction("Details", new { id });
    }

    [HttpPost]
    public IActionResult ServeDrinks(int id)
    {
        var order = _orderService.GetOrderByTable(id);
        if (order != null)
        {
            _orderService.ServeDrinkItems(order.OrderNumber);
        }
        return RedirectToAction("Details", new { id });
    }


}

