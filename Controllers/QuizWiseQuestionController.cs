using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;

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

        [HttpPost]
        public ActionResult AddQuizWiseQuestion(QuizWiseQuestionModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("QuizWiseQuestionList");
            }

            return View("QuizWiseQuestionForm", model);
        }
    }
}
