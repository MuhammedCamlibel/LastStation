using System.ComponentModel.DataAnnotations;

namespace shopapp.webui.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        //public string UserName { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;}

        public string ReturnUrl { get; set; }
    }
}