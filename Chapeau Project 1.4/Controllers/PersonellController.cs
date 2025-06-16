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
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear(); // Clear login session
            return RedirectToAction("Login", "Personell");
        }


        [HttpPost]
        public IActionResult GetByLoginCredentials(LoginModel loginModel)
        {
            string controller;
            string action;

            // Call service to validate credentials.
            // If successful, it returns a Personell object and sets the controller and action using 'out' parameters.
            //out parameters allow to return mutltiple values, then it redirects to the according controller / method
            Personell personell = _personellService.TryLogin(loginModel.Username, loginModel.Password, out controller, out action);

            // If login failed, show error and stay on login page
            if (personell == null)
            {
                ViewBag.ErrorMessage = "Bad username/password combination!";
                return View("Login", loginModel);
            }

            // Store login info in session for later use for e.g. in navbar
            HttpContext.Session.SetString("LoggedInUsername", personell.Username);
            HttpContext.Session.SetString("UserRole", personell.Role);

            // Redirect to the appropriate page based on the user's role
            return RedirectToAction(action, controller);
        }

    }
}
