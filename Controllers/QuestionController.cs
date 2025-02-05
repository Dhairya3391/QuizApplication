using Microsoft.AspNetCore.Mvc;

namespace QuizApplication.Controllers
{
    public class QuestionController : Controller
    {
        public IActionResult QuestionForm()
        {
            return View();
        }
        public IActionResult QuestionList()
        {
            return View();
        }
    }
}
