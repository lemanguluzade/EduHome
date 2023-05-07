using System.ComponentModel.DataAnnotations;

namespace EduHomee.ViewModels
{
    public class ResetPasswordVM
    {
       
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }
       

    }
}
