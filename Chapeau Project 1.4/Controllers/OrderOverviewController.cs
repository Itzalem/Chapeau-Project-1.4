using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.OrderOverview;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class OrderOverviewController : Controller
    {
		private readonly IOrderOverviewService _orderOverviewService;

		public OrderOverviewController(IOrderOverviewService orderOverviewService)
		{
			_orderOverviewService = orderOverviewService;
		}

		public IActionResult Index(int table)
        {
			List<OverviewItem> drinks = _orderOverviewService.DisplayOrderDrinks(table);
			List<OverviewItem> dishes = _orderOverviewService.DisplayOrderDishes(table);

			List<OverviewItem> order = new List<OverviewItem>();

			foreach (OverviewItem drink in drinks)
				order.Add(drink);
			foreach (OverviewItem dish in dishes)
				order.Add(dish);

			ViewBag.TableNumber = table;

			return View(order);
        }

        [HttpGet]
        public ActionResult GetTable()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTable(int table)
        {
            Index(table);

            return RedirectToAction("DisplayOrder", table);
        }
    }
}
