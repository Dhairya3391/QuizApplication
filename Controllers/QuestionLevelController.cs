using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;
using QuizApplication.Models;

namespace QuizApplication.Controllers
{
    public class QuestionLevelController : Controller
    {
        public readonly DbConfiguration _dbConfiguration;

        public QuestionLevelController(DbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public IActionResult QuestionLevelForm()
        {
            return View();
        }
        public IActionResult QuestionLevelList()
        {
            return View(_dbConfiguration.GetAllData("PR_QuestionLevel_SelectAll"));
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
