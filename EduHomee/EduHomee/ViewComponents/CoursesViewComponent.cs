using EduHomee.DAL;
using EduHomee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomee.ViewComponents
{
    public class CoursesViewComponent: ViewComponent
    {
        private readonly AppDbContext _db;
        public CoursesViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult>InvokeAsync(int take)
        {
            List<Course> courses = new List<Course>();
            if (take == 0)
            {
                courses = await _db.Courses.ToListAsync();
                return View(courses);
            }
            else 
            {
                courses = await _db.Courses.Take(take).ToListAsync();
            }
                 return View(courses);
        }
    }
}
