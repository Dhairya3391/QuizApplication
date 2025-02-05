using Microsoft.AspNetCore.Mvc;

namespace QuizApplication.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserList()
        {
            return View();
        }
    }
}
