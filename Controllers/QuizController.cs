using Microsoft.AspNetCore.Mvc;

namespace QuizApplication.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult QuizForm()
        {
            return View();
        }
        public IActionResult QuizList()
        {
            return View();
        }
    }
}
