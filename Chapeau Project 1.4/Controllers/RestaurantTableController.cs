using Chapeau_Project_1._4.Services.RestaurantTableService;
using Microsoft.AspNetCore.Mvc;

public class RestaurantTableController : Controller
{
    private readonly IRestaurantTableService _tableService;

    public RestaurantTableController(IRestaurantTableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet]
    public IActionResult Overview()
    {
        var tables = _tableService.GetAllTables();
        return View(tables);
    }
}

