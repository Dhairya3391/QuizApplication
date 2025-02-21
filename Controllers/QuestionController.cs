using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;
using QuizApplication.Models;
using QuizApplication.QuestionCRUD;

namespace QuizApplication.Controllers
{
    public class QuestionController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;
        private readonly QuestionCRUD.QuestionCRUD questionCRUD;
        public QuestionController(DbConfiguration dbConfiguration,QuestionCRUD.QuestionCRUD questionCRUD)
        {
            this._dbConfiguration = dbConfiguration;
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

        [HttpPost]
        public ActionResult AddQuestion(QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = questionCRUD.AddQuestion(model);

                if (isInserted)
                {
                    TempData["SuccessMessage"] = "question added successfully.";
                    return RedirectToAction("QuestionList");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while adding question.";
                }
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
            DataTable dt = questionCRUD.EditQuestion(id);

            if (dt.Rows.Count > 0)
            {

                QuestionModel model = new QuestionModel
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
                    UserID = Convert.ToInt32(dt.Rows[0]["UserID"].ToString()),
                };

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "Question not found.";
                return RedirectToAction("QuestionList");
            }
        }

        [HttpPost]
        public ActionResult UpdateQuestion(QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = questionCRUD.UpdateQuestion(model);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Question updated successfully.";
                    return RedirectToAction("QuestionList");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while updating question.";
                }
            }

            return View("EditQuestion", model);
        }



        [HttpGet]
        public ActionResult deleteQuestion(int id)
        {
            bool isDeleted = questionCRUD.DeleteQuestion(id);

            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Question deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error while deleting question.";
            }

            return RedirectToAction("QuestionList");
        }

    }
}
