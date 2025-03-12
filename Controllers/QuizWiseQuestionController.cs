using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfiguration;
using QuizApplication.Models;

namespace QuizApplication.Controllers;

public class QuizWiseQuestionController : Controller
{
    private readonly DbConfigruation.DbConfiguration _dbConfiguration;
    private readonly QuizWiseQuestionCRUD _quizWiseQuestion;

    public QuizWiseQuestionController(DbConfigruation.DbConfiguration dbConfiguration,
        QuizWiseQuestionCRUD quizWiseQuestion)
    {
        _dbConfiguration = dbConfiguration;
        _quizWiseQuestion = quizWiseQuestion;
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
        if (ModelState.IsValid) return RedirectToAction("QuizWiseQuestionList");

        return View("QuizWiseQuestionForm", model);
    }

    public IActionResult QuizWiseQuestionDetails(int quizId, string quizName, int totalQuestions)
    {
        return RedirectToAction("QuizWiseQuestionDetailsAction", "QuizWiseQuestion",
            new { quizId, quizName, totalQuestions });
    }

    public IActionResult QuizWiseQuestionDetailsAction(int quizId, string quizName, int totalQuestions)
    {
        TempData["QuizId"] = quizId;
        TempData["QuizName"]  = quizName;
        TempData["totalQuestions"] = totalQuestions;

        TempData.Keep("QuizId");
        TempData.Keep("QuizName");
        TempData.Keep("totalQuestions");

        return View("QuizWiseQuestionDetails", _quizWiseQuestion.GetQuizWiseQuestionDetails(quizId));
    }

    //public IActionResult AddQuizWiseQuestions(string quizName, int totalQuestions)
    //{
    //    //ViewBag.QuizName = quizName;
    //    //ViewBag.TotalQuestions = totalQuestions;
    //    //return View(_dbConfiguration.GetAllData("PR_Question_SelectAll"));
    //    //return View();
    //    var quizId = _quizWiseQuestion.GetQuizIdByName(quizName);
    //    ViewBag.QuizId = quizId;
    //    ViewBag.QuizName = quizName;
    //    ViewBag.TotalQuestions = totalQuestions;

    //    // Fetch already added questions
    //    var addedQuestions = _quizWiseQuestion.GetAddedQuestionIds(quizId);
    //    ViewBag.AddedQuestions = addedQuestions;

    //    // Fetch all available questions
    //    var allQuestions = _dbConfiguration.GetAllData("PR_Question_SelectAll");

    //    return View(allQuestions);
    //}

    private object GetDefaultValue(Type type)
    {
        if (type == typeof(string)) return "N/A";
        if (type == typeof(int) || type == typeof(long)) return 0;
        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return 0.0;
        if (type == typeof(DateTime)) return DateTime.MinValue;
        return DBNull.Value; // Return database NULL as last resort
    }

    public IActionResult AddQuizWiseQuestions(string quizName, int totalQuestions)
    {
        try
        {
            // Get Quiz ID by name
            var quizId = _quizWiseQuestion.GetQuizIdByName(quizName);
            if (quizId == 0)
            {
                TempData["ErrorMessage"] = "Invalid Quiz Name. No matching quiz found!";
                return RedirectToAction("QuizList"); // Redirect if quiz doesn't exist
            }

            ViewBag.QuizId = quizId;
            ViewBag.QuizName = quizName;
            ViewBag.TotalQuestions = totalQuestions;

            // Fetch already added questions
            var addedQuestions = _quizWiseQuestion.GetAddedQuestionIds(quizId);
            ViewBag.AddedQuestions = addedQuestions;

            // Fetch all available questions
            var allQuestions = _dbConfiguration.GetAllData("PR_Question_SelectAll");

            // Handle NULL values in DataTable
            foreach (DataRow row in allQuestions.Rows)
            {
                foreach (DataColumn col in allQuestions.Columns)
                {
                    if (row.IsNull(col))
                    {
                        row[col] = GetDefaultValue(col.DataType);
                    }
                }
            }

            return View(allQuestions);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            return RedirectToAction("QuizWiseQuestionList");
        }
    }

    [HttpPost]
    public IActionResult SaveQuizQuestions(int quizId, List<int> selectedQuestions)
    {
        var userId = 1;

        foreach (var questionId in selectedQuestions) _quizWiseQuestion.AddQuizWiseQuestion(quizId, questionId, userId);

        return RedirectToAction("QuizWiseQuestionList");
    }

    [HttpGet]
    public ActionResult DeleteQuizWiseQuestion(int quizWiseQuestionID)
    {
        var isDeleted = _quizWiseQuestion.DeleteQuizWiseQuestion(quizWiseQuestionID);

        if (isDeleted)
            TempData["SuccessMessage"] = "Question deleted successfully.";
        else
            TempData["ErrorMessage"] = "Error while deleting question.";

        //int quizId = Convert.ToInt32(TempData["QuizId"]);
        //string quizName = Convert.ToString(TempData["QuizName"]);
        //int totalQuestions = Convert.ToInt32(TempData["totalQuestions"]);

        int quizId = Convert.ToInt32(TempData.Peek("QuizId"));
        string quizName = Convert.ToString(TempData.Peek("QuizName"));
        int totalQuestions = Convert.ToInt32(TempData.Peek("totalQuestions"));


        //return RedirectToAction("QuizWiseQuestionDetailsAction", "QuizWiseQuestion", );
        return RedirectToAction("QuizWiseQuestionDetailsAction", "QuizWiseQuestion",
            new { quizId, quizName, totalQuestions });
    }
}