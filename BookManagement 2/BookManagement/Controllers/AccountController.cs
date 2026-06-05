using Microsoft.AspNetCore.Mvc;
using BookManagement.Models;

namespace BookManagement.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Username == "admin" && model.Password == "123")
            {
                TempData["Result"] = "Login success";
            }
            else
            {
                TempData["Result"]  = "Login failed";
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
