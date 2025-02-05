using Microsoft.AspNetCore.Mvc;

namespace QuizApplication.Controllers
{
    public class QuestionLevelController : Controller
    {
        public IActionResult QuestionLevelForm()
        {
            return View();
        }
        public IActionResult QuestionLevelList()
        {
            return View();
        }
    }
}
