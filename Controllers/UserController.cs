using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;

namespace QuizApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;
        private readonly UserCrud.UserCrud _userCrud;

        public UserController(DbConfiguration dbConfiguration, UserCrud.UserCrud userCrud)
        {
            _dbConfiguration = dbConfiguration;
            _userCrud = userCrud;
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            bool isDeleted = _userCrud.DeleteUser(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error while deleting user.";
            }
            return RedirectToAction("UserList");
        }
        
        public IActionResult UserList()
        {
            return View(_dbConfiguration.GetAllData("PR_User_SelectAll"));
        }
    }
}
