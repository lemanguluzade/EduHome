using EduHomee.DAL;
using EduHomee.Models;
using EduHomee.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomee.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
           
            HomeVM homeVM = new HomeVM
            {
                Sliders = await _db.Sliders.ToListAsync(),
                Services = await _db.Services.ToListAsync(),
                
                Feedbacks= await _db.Feedbacks.ToListAsync(),
                About= await _db.Abouts.FirstOrDefaultAsync(),
                Blogs = await _db.Blogs.ToListAsync(),
                Events = await _db.Events.ToListAsync()
            };   
            return View(homeVM);
        }

        

        
        public IActionResult Error()
        {
            return View();
        }
    }
}
