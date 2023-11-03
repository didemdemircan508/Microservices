using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SigninInput
    {
        [Required]
        [DisplayName("Email Adresiniz")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Şifreniz")]
        public string Password { get; set; }


        [DisplayName("Beni Hatırla")]
        public bool IsRemember { get; set; }
    }
}
