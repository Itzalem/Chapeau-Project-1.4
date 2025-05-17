using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services;
using Microsoft.AspNetCore.Mvc;
using System.ClientModel.Primitives;

namespace Chapeau_Project_1._4.Controllers
{
    public class PersonellController : Controller
    {
        private readonly IPersonellService _personellService;

        public PersonellController(IPersonellService personellService)
        {
            _personellService = personellService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult GetByLoginCredentials(LoginModel loginModel)
        {
            Personell? personell = _personellService.GetByLoginCredentials(loginModel.Username, loginModel.Password);

            if (personell == null)
            {
                ViewBag.ErrorMessage = "Bad username/password combination!";
                return View("Login", loginModel);
            }
            else
            {
                HttpContext.Session.SetString("LoggedInUsername", personell.Username);
                HttpContext.Session.SetString("UserRole", personell.Role);

                return RedirectToAction("Index", "Users");
            }   
        }
        
    }
}
