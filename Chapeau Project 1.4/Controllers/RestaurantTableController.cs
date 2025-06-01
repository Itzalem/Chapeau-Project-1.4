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

}

