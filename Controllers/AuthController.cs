using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfiguration;
using QuizApplication.Models;

namespace QuizApplication.Controllers;

public class AuthController : Controller
{
    private readonly UserCrud _userCrud;

    public AuthController(UserCrud userCrud)
    {
        _userCrud = userCrud;
    }

    public IActionResult Register()
    {
        if (HttpContext.Session.GetInt32("UserId").HasValue) return RedirectToAction("Index", "Home");
        return View(new User());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(User user)
    {
        if (HttpContext.Session.GetInt32("UserId").HasValue) return RedirectToAction("Index", "Home");

        if (ModelState.IsValid)
            try
            {
                var isRegistered = _userCrud.RegisterUser(user);
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

        return View(user);
    }

    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("UserId").HasValue) return RedirectToAction("Index", "Home");
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (HttpContext.Session.GetInt32("UserId").HasValue) return RedirectToAction("Index", "Home");

        if (ModelState.IsValid)
        {
            var loggedInUser = _userCrud.LoginUser(model.Email, model.Password);
            if (loggedInUser != null)
            {
                HttpContext.Session.SetInt32("UserId", loggedInUser.UserID);
                HttpContext.Session.SetString("Username", loggedInUser.Username);
                HttpContext.Session.SetString("IsAdmin", loggedInUser.IsAdmin.ToString());
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Invalid email or password.";
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["Success"] = "You have been logged out successfully.";
        return RedirectToAction("Login");
    }
}