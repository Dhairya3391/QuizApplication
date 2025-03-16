using System.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfiguration;
using QuizApplication.Models;


namespace QuizApplication.Controllers;

public class QuizController : Controller
{
    private readonly DbConfigruation.DbConfiguration _dbConfiguration;
    private readonly QuizCRUD _quizCRUD;
    private readonly QuizWiseQuestionCRUD _quizWiseQuestionCRUD;

    public QuizController(DbConfigruation.DbConfiguration dbConfiguration, QuizCRUD quizCRUD, QuizWiseQuestionCRUD quizWiseQuestionCRUD)
    {
        _dbConfiguration = dbConfiguration;
        _quizCRUD = quizCRUD;
        _quizWiseQuestionCRUD = quizWiseQuestionCRUD;
    }

    // GET: Display the form to create a new quiz
    public IActionResult QuizForm()
    {
        return View();
    }

    // GET: Display the list of all quizzes
    public IActionResult QuizList()
    {
        return View(_dbConfiguration.GetAllData("PR_Quiz_SelectAll"));
    }

    [HttpGet]
    public IActionResult QuizList(string quizName = null, int? totalQuestion = null, DateTime? quizDate = null)
    {
        DataTable dt;
        if (!string.IsNullOrEmpty(quizName) || totalQuestion.HasValue || quizDate.HasValue)
            dt = _quizCRUD.SearchQuizzes(quizName, totalQuestion, quizDate);
        else
            dt = _dbConfiguration.GetAllData("PR_Quiz_SelectAll");

        ViewBag.QuizName = quizName;
        ViewBag.TotalQuestion = totalQuestion;
        ViewBag.QuizDate = quizDate;

        return View(dt);
    }

    // POST: Add a new quiz
    [HttpPost]
    public ActionResult AddQuiz(QuizModel model)
    {
        Console.WriteLine("ADDQUIZ");
        if (ModelState.IsValid)
        {
            Console.WriteLine("ADDQUIZzz");
            //var isInserted = _quizCRUD.AddQuiz(model);
            int quizId = _quizCRUD.AddQuiz(model);
            if (quizId > 0) { 
                var isInsertedQWQ = _quizWiseQuestionCRUD.AddQuizWiseQuestionRef(quizId);
                Console.WriteLine(quizId);
                if(isInsertedQWQ)
                {
                    TempData["SuccessMessage"] = "Quiz added successfully.";
                    return RedirectToAction("QuizList");
                }
            }

            TempData["ErrorMessage"] = "Error while adding quiz.";
        }

        return View("QuizForm", model);
    }

    // GET: Display the form to edit an existing quiz
    [HttpGet]
    public ActionResult EditQuiz(int id)
    {
        var dt = _quizCRUD.EditQuiz(id);
        if (dt.Rows.Count > 0)
        {
            var model = new QuizModel
            {
                QuizID = Convert.ToInt32(dt.Rows[0]["QuizID"]),
                QuizName = dt.Rows[0]["QuizName"].ToString(),
                TotalQuestions = Convert.ToInt32(dt.Rows[0]["TotalQuestions"]),
                QuizDate = Convert.ToDateTime(dt.Rows[0]["QuizDate"]),
                Created = Convert.ToDateTime(dt.Rows[0]["Created"]),
                Modified = Convert.ToDateTime(dt.Rows[0]["Modified"])
            };
            return View(model);
        }   

        TempData["ErrorMessage"] = "Quiz not found.";
        return RedirectToAction("QuizList");
    }

    // POST: Update an existing quiz
    [HttpPost]
    public ActionResult UpdateQuiz(QuizModel model)
    {
        if (ModelState.IsValid)
        {
            var isUpdated = _quizCRUD.UpdateQuiz(model);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Quiz updated successfully.";
                return RedirectToAction("QuizList");
            }

            TempData["ErrorMessage"] = "Error while updating quiz.";
        }

        return View("EditQuiz", model);
    }

    // GET: Delete a quiz
    [HttpGet]
    public ActionResult DeleteQuiz(int id)
    {
        var isDeleted = _quizCRUD.DeleteQuiz(id);
        if (isDeleted)
            TempData["SuccessMessage"] = "Quiz deleted successfully.";
        else
            TempData["ErrorMessage"] = "Error while deleting quiz.";
        return RedirectToAction("QuizList");
    }
    public IActionResult ExportQuizList(string quizName = null, int? totalQuestion = null, DateTime? quizDate = null)
    {
        DataTable dt;
        if (!string.IsNullOrEmpty(quizName) || totalQuestion.HasValue || quizDate.HasValue)
            dt = _quizCRUD.SearchQuizzes(quizName, totalQuestion, quizDate);
        else
            dt = _dbConfiguration.GetAllData("PR_Quiz_SelectAll");

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("QuizList");
        worksheet.Cell(1, 1).InsertTable(dt);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();
        return File(content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "QuizList.xlsx");
    }


    [HttpPost]
    public JsonResult SearchQuizzes(string quizName, int? totalQuestion, DateTime? quizDate)
    {
        var dt = _quizCRUD.SearchQuizzes(quizName, totalQuestion, quizDate);
        return Json(new { data = dt.AsEnumerable().Select(r => r.ItemArray) });
    }
}