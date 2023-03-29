using EduHomee.DAL;
using EduHomee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduHomee.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;

        public IActionResult Index()
        {
            return View();
        }
    }
}
