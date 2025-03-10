using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfiguration;
using QuizApplication.Models;

namespace QuizApplication.Controllers;

public class QuestionLevelController : Controller
{
    private readonly DbConfigruation.DbConfiguration _dbConfiguration;
    private readonly QuestionLevelCRUD questionLevelCRUD;

    public QuestionLevelController(DbConfigruation.DbConfiguration dbConfiguration, QuestionLevelCRUD questionLevelCRUD)
    {
        _dbConfiguration = dbConfiguration;
        this.questionLevelCRUD = questionLevelCRUD;
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
            var isInserted = questionLevelCRUD.AddQuestionLevel(model);
            if (isInserted)
            {
                TempData["SuccessMessage"] = "Question level added successfully.";
                return RedirectToAction("QuestionLevelList");
            }

            TempData["ErrorMessage"] = "Error while adding question level.";
        }

        return View("QuestionLevelForm", model);
    }

    [HttpGet]
    public ActionResult EditQuestionLevel(int id)
    {
        var dt = questionLevelCRUD.EditQuestionLevel(id);
        if (dt.Rows.Count > 0)
        {
            var model = new QuestionLevelsModel
            {
                QuestionLevelID = Convert.ToInt32(dt.Rows[0]["QuestionLevelID"]),
                QuestionLevel = dt.Rows[0]["QuestionLevel"].ToString(),
                UserID = Convert.ToInt32(dt.Rows[0]["UserID"]),
                Created = Convert.ToDateTime(dt.Rows[0]["Created"]),
                Modified = Convert.ToDateTime(dt.Rows[0]["Modified"])
            };
            return View(model);
        }

        TempData["ErrorMessage"] = "Question level not found.";
        return RedirectToAction("QuestionLevelList");
    }

    [HttpPost]
    public ActionResult UpdateQuestionLevel(QuestionLevelsModel model)
    {
        if (ModelState.IsValid)
        {
            var isUpdated = questionLevelCRUD.UpdateQuestionLevel(model);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Question level updated successfully.";
                return RedirectToAction("QuestionLevelList");
            }

            TempData["ErrorMessage"] = "Error while updating question level.";
        }

        return View("EditQuestionLevel", model);
    }

    [HttpGet]
    public ActionResult DeleteQuestionLevel(int id)
    {
        var isDeleted = questionLevelCRUD.DeleteQuestionLevel(id);
        if (isDeleted)
            TempData["SuccessMessage"] = "Question level deleted successfully.";
        else
            TempData["ErrorMessage"] = "Error while deleting question level.";
        return RedirectToAction("QuestionLevelList");
    }
}