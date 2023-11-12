using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace FreeCourse.Web.Models.Baskets
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; }

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public decimal Price { get; set; }

        private decimal? DiscountAppliedPrice;



        public decimal GetCurrentPrice
        {
            get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;

        }
        public void AppliedDiscount(decimal discountPrice)
        { 
          DiscountAppliedPrice=discountPrice;
        }



    }
}
