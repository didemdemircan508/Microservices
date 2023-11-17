using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Orders
{
    public class CheckoutInfoInput
    {
        [Display(Name ="il")]
        public string Province { get;  set; }

        [Display(Name = "ilçe")]
        public string District { get;  set; }

        [Display(Name = "cadde")]
        public string Street { get;  set; }

        [Display(Name = "posta kodu")]

        public string ZipCode { get;  set; }

        [Display(Name = "adres")]
        public string Line { get;  set; }

        [Display(Name = "Kart İsim")]
        public string CardName { get; set; }

        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Display(Name = "Son .Kull.Tarihi")]
        public string Expiration { get; set; }

        [Display(Name = "CVV")]
        public string CVV { get; set; }

        


    }
}
