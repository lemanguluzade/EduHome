using System;

namespace EduHomee.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
    }
}
