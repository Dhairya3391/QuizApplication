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
        ViewBag.QuizName = quizName;
        ViewBag.TotalQuestions = totalQuestions;
        return View("QuizWiseQuestionDetails", _quizWiseQuestion.GetQuizWiseQuestionDetails(quizId));
    }

    public IActionResult AddQuizWiseQuestions(string quizName, int totalQuestions)
    {
        //ViewBag.QuizName = quizName;
        //ViewBag.TotalQuestions = totalQuestions;
        //return View(_dbConfiguration.GetAllData("PR_Question_SelectAll"));
        //return View();
        var quizId = _quizWiseQuestion.GetQuizIdByName(quizName);
        ViewBag.QuizId = quizId;
        ViewBag.QuizName = quizName;
        ViewBag.TotalQuestions = totalQuestions;

        // Fetch already added questions
        var addedQuestions = _quizWiseQuestion.GetAddedQuestionIds(quizId);
        ViewBag.AddedQuestions = addedQuestions;

        // Fetch all available questions
        var allQuestions = _dbConfiguration.GetAllData("PR_Question_SelectAll");

        return View(allQuestions);
    }

    //[HttpPost]
    //public IActionResult SaveQuizQuestions(int quizId, List<int> selectedQuestions)
    //{
    //    if (selectedQuestions == null || selectedQuestions.Count == 0)
    //    {
    //        TempData["ErrorMessage"] = "Please select at least one question.";
    //        return RedirectToAction("QuizWiseQuestionDetails", new { quizId = quizId });
    //    }

    //    try
    //    {
    //        _quizWiseQuestion.SaveSelectedQuestions(quizId, selectedQuestions);
    //        TempData["SuccessMessage"] = "Questions added successfully!";
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ErrorMessage"] = "Error adding questions: " + ex.Message;
    //    }

    //    return RedirectToAction("QuizWiseQuestionList");
    //}


    [HttpPost]
    public IActionResult SaveQuizQuestions(int quizId, List<int> selectedQuestions)
    {
        var userId = 1; // Assume logged-in user ID

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

        return RedirectToAction("QuizWiseQuestionList");
    }
}