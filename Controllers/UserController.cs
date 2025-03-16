using Microsoft.AspNetCore.Mvc;
using QuizApplication.DbConfiguration;

namespace QuizApplication.Controllers;

public class UserController : Controller
{
    private readonly DbConfigruation.DbConfiguration _dbConfiguration;
    private readonly UserCrud _userCrud;

    public UserController(DbConfigruation.DbConfiguration dbConfiguration, UserCrud userCrud)
    {
        _dbConfiguration = dbConfiguration;
        _userCrud = userCrud;
    }

    [HttpGet]
    public ActionResult DeleteUser(int id)
    {
        var isDeleted = _userCrud.DeleteUser(id);
        if (isDeleted)
            TempData["SuccessMessage"] = "User deleted successfully.";
        else
            TempData["ErrorMessage"] = "Error while deleting user.";
        return RedirectToAction("UserList");
    }

    public IActionResult UserList()
    {

        return View(_userCrud.Users("PR_User_SelectAll"));
    }
}