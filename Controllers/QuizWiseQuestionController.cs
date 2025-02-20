using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;
using QuizApplication.Models;

namespace QuizApplication.Controllers
{
    public class QuizWiseQuestionController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;
        public QuizWiseQuestionController(DbConfiguration dbConfiguration)
        {
            this._dbConfiguration = dbConfiguration;

        }
        public IActionResult QuizWiseQuestionForm()
        {
            return View();
        }
        public IActionResult QuizWiseQuestionList()
        {
            return View(_dbConfiguration.GetAllData("PR_QuizQuestionsWise_SelectAll"));

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
