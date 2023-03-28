using Microsoft.AspNetCore.Mvc;

namespace EduHomee.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
