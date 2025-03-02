using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;
using QuizApplication.Models;
using QuizApplication.QuizWiseQuestionCRUDCRUD;

namespace QuizApplication.Controllers
{
    public class QuizWiseQuestionController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;
        private readonly QuizWiseQuestionCRUD _quizWiseQuestion;
        public QuizWiseQuestionController(DbConfiguration dbConfiguration, QuizWiseQuestionCRUD quizWiseQuestion)
        {
            this._dbConfiguration = dbConfiguration;
            this._quizWiseQuestion = quizWiseQuestion;
        }

        public IActionResult QuizWiseQuestionList()
        {
            return View(_dbConfiguration.GetAllData("PR_QuizQuestionsWise_SelectAll"));
        }

        public IActionResult QuizWiseQuestionForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddQuizWiseQuestionEntry(QuizWiseQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("QuizWiseQuestionList");
            }

            return View("QuizWiseQuestionForm", model);
        }

        public IActionResult QuizWiseQuestionDetails(int quizId, string quizName,int totalQuestions)
        {
            return RedirectToAction("QuizWiseQuestionDetailsAction", "QuizWiseQuestion",
        new { quizId = quizId, quizName = quizName, totalQuestions = totalQuestions });
        }

        public IActionResult QuizWiseQuestionDetailsAction(int quizId, string quizName, int totalQuestions)
        {
            ViewBag.QuizName = quizName;
            ViewBag.TotalQuestions = totalQuestions;
            return View("QuizWiseQuestionDetails", _quizWiseQuestion.GetQuizWiseQuestionDetails(quizId));
        }

        public IActionResult AddQuizWiseQuestions(string quizName, int totalQuestions) {
            ViewBag.QuizName = quizName;
            ViewBag.TotalQuestions = totalQuestions;
            return View(_dbConfiguration.GetAllData("PR_Question_SelectAll"));
            //return View();
        }

    }
}
