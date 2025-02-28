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
        
        // GET: Login
        public IActionResult Login()
        {
            System.Diagnostics.Debug.WriteLine("Login GET called");
            return View(new LoginViewModel());
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("Login POST called");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"Attempting login with Email: {model.Email}");
                var loggedInUser = _userCrud.LoginUser(model.Email, model.Password);
                if (loggedInUser != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Login successful: Id={loggedInUser.UserID}, Username={loggedInUser.Username}");
                    HttpContext.Session.SetInt32("UserId", loggedInUser.UserID);
                    HttpContext.Session.SetString("Username", loggedInUser.Username);
                    System.Diagnostics.Debug.WriteLine("Session set, redirecting to Home/Index");
                    return RedirectToAction("Index", "Home");
                }
                TempData["Error"] = "Invalid email or password.";
                System.Diagnostics.Debug.WriteLine("Login failed: Invalid credentials");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                System.Diagnostics.Debug.WriteLine("Login invalid: " + string.Join(", ", errors));
            }
            System.Diagnostics.Debug.WriteLine("Returning Login view");
            return View(model);
        }

        // GET: Logout
        public IActionResult Logout()
        {
            System.Diagnostics.Debug.WriteLine("Logout called");
            HttpContext.Session.Clear();
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}