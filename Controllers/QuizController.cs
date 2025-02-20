using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;
using QuizApplication.DbConfigruation;
using System.Data.SqlClient;
using System.Data;
namespace QuizApplication.Controllers
{
    public class QuizController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;

        public QuizController(DbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public IActionResult QuizForm()
        {
            return View();
        }
        public IActionResult QuizList()
        {
            return View(_dbConfiguration.GetAllData("PR_Quiz_SelectAll"));
            
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
