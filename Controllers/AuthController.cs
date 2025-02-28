using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;
using QuizApplication.UserCrud;

namespace QuizApplication.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserCrud.UserCrud _userCrud;

        public AuthController(UserCrud.UserCrud userCrud)
        {
            _userCrud = userCrud;
        }

        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isRegistered = _userCrud.RegisterUser(user);
                    if (isRegistered)
                    {
                        TempData["Success"] = "Registration successful! Please log in.";
                        return RedirectToAction("Login");
                    }
                    TempData["Error"] = "Registration failed.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                }
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}