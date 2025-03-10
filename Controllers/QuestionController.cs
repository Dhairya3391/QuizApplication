using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfiguration;
using QuizApplication.Models;

namespace QuizApplication.Controllers;

public class QuestionController : Controller
{
    private readonly DbConfigruation.DbConfiguration _dbConfiguration;
    private readonly QuestionCRUD questionCRUD;

    public QuestionController(DbConfigruation.DbConfiguration dbConfiguration, QuestionCRUD questionCRUD)
    {
        _dbConfiguration = dbConfiguration;
        this.questionCRUD = questionCRUD;
    }

    public IActionResult QuestionForm()
    {
        return View();
    }

    public IActionResult QuestionList()
    {
        return View(_dbConfiguration.GetAllData("PR_Question_SelectAll"));
    }

    [HttpGet]
    public IActionResult QuestionList(string questionText = null, string correctOptions = null,
        int? questionMarks = null)
    {
        DataTable dt;
        if (!string.IsNullOrEmpty(questionText) || !string.IsNullOrEmpty(correctOptions) || questionMarks.HasValue)
            dt = questionCRUD.SearchQuestions(questionText, correctOptions, questionMarks);
        else
            dt = _dbConfiguration.GetAllData("PR_Question_SelectAll");

        // Set ViewBag for preserving search values
        ViewBag.QuestionText = questionText;
        ViewBag.CorrectOptions = correctOptions;
        ViewBag.QuestionMarks = questionMarks;

        return View(dt);
    }

    [HttpPost]
    public ActionResult AddQuestion(QuestionModel model)
    {
        if (ModelState.IsValid)
        {
            var isInserted = questionCRUD.AddQuestion(model);

            if (isInserted)
            {
                TempData["SuccessMessage"] = "question added successfully.";
                return RedirectToAction("QuestionList");
            }

            TempData["ErrorMessage"] = "Error while adding question.";
        }

        //return View("QuestionForm");
        return View("QuestionForm", model);
    }

    //[HttpGet]
    //public ActionResult editQuestion(int id)
    //{
    //    //return View(_dbConfiguration.GetAllData("PR_Question_SelectByPK"));

    //    return View(questionCRUD.EditQuestion(id));
    //    //if (isDeleted)
    //    //{
    //    //    TempData["SuccessMessage"] = "Question deleted successfully.";
    //    //}
    //    //else
    //    //{
    //    //    TempData["ErrorMessage"] = "Error while deleting question.";
    //    //}

    //    //return RedirectToAction("QuestionList");
    //}
    [HttpGet]
    public ActionResult EditQuestion(int id)
    {
        var dt = questionCRUD.EditQuestion(id);

        if (dt.Rows.Count > 0)
        {
            var model = new QuestionModel
            {
                QuestionID = Convert.ToInt32(dt.Rows[0]["QuestionID"]),
                QuestionText = dt.Rows[0]["QuestionText"].ToString(),
                OptionA = dt.Rows[0]["OptionA"].ToString(),
                OptionB = dt.Rows[0]["OptionB"].ToString(),
                OptionC = dt.Rows[0]["OptionC"].ToString(),
                OptionD = dt.Rows[0]["OptionD"].ToString(),
                CorrectOption = dt.Rows[0]["CorrectOption"].ToString(),
                QuestionMarks = Convert.ToInt32(dt.Rows[0]["QuestionMarks"]),
                IsActive = dt.Columns.Contains("IsActive") ? Convert.ToBoolean(dt.Rows[0]["IsActive"]) : false,
                UserID = Convert.ToInt32(dt.Rows[0]["UserID"].ToString())
            };

            return View(model);
        }

        TempData["ErrorMessage"] = "Question not found.";
        return RedirectToAction("QuestionList");
    }

    [HttpPost]
    public ActionResult UpdateQuestion(QuestionModel model)
    {
        if (ModelState.IsValid)
        {
            var isUpdated = questionCRUD.UpdateQuestion(model);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Question updated successfully.";
                return RedirectToAction("QuestionList");
            }

            TempData["ErrorMessage"] = "Error while updating question.";
        }

        return View("EditQuestion", model);
    }


    [HttpGet]
    public ActionResult deleteQuestion(int id)
    {
        var isDeleted = questionCRUD.DeleteQuestion(id);

        if (isDeleted)
            TempData["SuccessMessage"] = "Question deleted successfully.";
        else
            TempData["ErrorMessage"] = "Error while deleting question.";

        return RedirectToAction("QuestionList");
    }
}