using Microsoft.AspNetCore.Mvc;

namespace EduHomee.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
