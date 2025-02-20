using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfigruation;

namespace QuizApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly DbConfiguration _dbConfiguration;

        public UserController(DbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public IActionResult UserList()
        {
            return View(_dbConfiguration.GetAllData("PR_User_SelectAll"));
        }
    }
}
