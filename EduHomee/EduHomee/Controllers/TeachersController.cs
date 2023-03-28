using Microsoft.AspNetCore.Mvc;

namespace EduHomee.Controllers
{
    public class TeachersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
