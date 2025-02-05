using Microsoft.AspNetCore.Mvc;

namespace QuizApplication.Controllers
{
    public class QuizWiseQuestionController : Controller
    {
        public IActionResult QuizWiseQuestionForm()
        {
            return View();
        }
        public IActionResult QuizWiseQuestionList()
        {
            return View();
        }
    }
}
