using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;

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

        [HttpPost]
        public ActionResult AddQuiz(QuizModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return Json(new { success = false, errors });
            //}
            if (ModelState.IsValid)
            {

                return RedirectToAction("QuizList");
            }

            return View("QuizForm", model);
        }

    }
}
