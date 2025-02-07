using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;

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

        [HttpPost]
        public ActionResult AddQuestionLevel(QuestionLevelsModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("QuestionLevelList");
            }

            return View("QuestionLevelForm");
        }
    }
}
