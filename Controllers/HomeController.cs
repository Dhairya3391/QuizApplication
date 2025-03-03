using Microsoft.AspNetCore.Mvc;
using QuizApplication.Services;

namespace QuizApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashboardService _dashboardService;

        public HomeController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = _dashboardService.GetDashboardData();
            return View(model);
        }
    }
}