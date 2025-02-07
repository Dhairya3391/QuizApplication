using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;

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

        [HttpPost]
        public ActionResult AddQuestion(QuestionModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("QuestionList");
            }

            return View("QuestionForm");
        }

    }
}
