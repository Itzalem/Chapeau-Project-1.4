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
        var viewModels = _tableService.GetTablesWithOrderStatus(_orderService);
        return View(viewModels);
    }

    [HttpGet]
    public IActionResult GetTable()
    {
        List<RestaurantTable> tables = _tableService.GetAllTables();

        return View(tables);
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

        _tableService.UpdateTableOccupancy(id, !table.IsOccupied);
        return RedirectToAction("Overview");
    }


}

