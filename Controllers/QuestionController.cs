using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;
using QuizApplication.Models;

namespace QuizApplication.Controllers
{
    public class QuestionController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;

        public QuestionController(DbConfiguration dbConfiguration)
        {
            this._dbConfiguration = dbConfiguration;
        }

        public IActionResult QuestionForm()
        {
            return View();
        }
        public IActionResult QuestionList()
        {
            return View(_dbConfiguration.GetAllData("PR_Question_SelectAll"));
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
