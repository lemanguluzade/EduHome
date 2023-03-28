using Microsoft.AspNetCore.Mvc;

namespace EduHomee.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
