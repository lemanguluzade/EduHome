using System.ComponentModel.DataAnnotations;

namespace EduHomee.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
