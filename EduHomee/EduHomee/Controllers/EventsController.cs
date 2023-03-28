using Microsoft.AspNetCore.Mvc;

namespace EduHomee.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
