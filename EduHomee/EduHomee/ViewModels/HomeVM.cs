using EduHomee.Models;
using System.Collections.Generic;

namespace EduHomee.ViewModels
{
    public class HomeVM
    {
        public List<Service> Services { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Course> Courses { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public About About { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Event> Events { get; set; }
    }
}
