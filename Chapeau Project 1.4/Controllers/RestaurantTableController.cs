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

    // Displays the overview of all restaurant tables with their current statuses
    [HttpGet]
    public IActionResult Overview()
    {
        var viewModels = _tableService.GetTablesWithOrderStatus(); // Get list of table view models
        return View(viewModels); // Pass them to the view
    }

    // Shows detailed info for a single table
    [HttpGet]
    public IActionResult Details(int id)
    {
        var table = _tableService.GetTableByNumber(id); // Get the table by its number
        if (table == null)
            return NotFound(); // Show 404 if table not found

        var order = _orderService.GetOrderByTable(id); // Get the order for this table
        bool canBeFreed = _tableService.CanBeFreed(table, order); // Check if the table can be toggled manually

        ViewBag.CanToggle = canBeFreed; // Pass toggle status to view
        ViewBag.TableNumber = id;

        if (order != null)
        {
            order.OrderItems = _orderService.GetOrderItems(order.OrderNumber); // Load order items
            ViewBag.Order = order; // Pass order info to view
        }

        return View(table); // Pass table info to view
    }

    // Toggles the occupancy status of a table (e.g. free/occupied)
    [HttpPost]
    public IActionResult ToggleOccupancy(int id)
    {
        var table = _tableService.GetTableByNumber(id); // Get the table
        if (table == null)
            return NotFound();

        var order = _orderService.GetOrderByTable(id); // Get the current order on the table
        bool success = _tableService.TryToggleOccupancy(table, order); // Try to toggle (free or occupy)

        if (!success)
        {
            // If toggle fails (e.g. unpaid order), show error on return to details
            TempData["ToggleError"] = "Table has unpaid or active orders and cannot be marked free.";
            return RedirectToAction("Details", new { id });
        }

        return RedirectToAction("Overview"); // Redirect back to table list
    }

    // Marks food items on this table's order as served
    [HttpPost]
    public IActionResult ServeFood(int id)
    {
        var order = _orderService.GetOrderByTable(id); // Get order for this table
        if (order != null)
        {
            _orderService.ServeFoodItems(order.OrderNumber); // Mark food items as served
        }
        return RedirectToAction("Details", new { id }); // Reload table details
    }

    // Marks drink items on this table's order as served
    [HttpPost]
    public IActionResult ServeDrinks(int id)
    {
        var order = _orderService.GetOrderByTable(id); // Get order for this table
        if (order != null)
        {
            _orderService.ServeDrinkItems(order.OrderNumber); // Mark drink items as served
        }
        return RedirectToAction("Details", new { id }); // Reload table details
    }
}
