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

		public IActionResult DisplayOrder(int? table)
        {
			List<OverviewItem> order = new List<OverviewItem>();

			ViewBag.TableNumber = table;

			return View(order);
        }
    }
}
