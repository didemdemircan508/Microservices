namespace FreeCourse.Web.Models.Baskets
{
    public class BasketViewModel
    {
        private List<BasketItemViewModel> _basketItems;
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }

        public string UserId { get; set; }

        public string? DiscountCode { get; set; } 

        public int? DiscountRate { get; set; }

        //public List<BasketItemViewModel> basketItems { get { return _basketItems; } set { _basketItems = value; } }




        public List<BasketItemViewModel> basketItems
        {
            get
            {
                if (HasDiscount)
                {
                    //kurs 100 tl inidirim oranı yüzde 10 
                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
                    });
                }
                return _basketItems;



            }
            set
            {
                _basketItems = value;
            }


        }


        public decimal TotalPrice
        {
            get => _basketItems.Sum(x => x.GetCurrentPrice);
        }

        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode)&&DiscountRate.HasValue;
        }

        public void CancelDiscount()
        {
            DiscountCode = string.Empty;
            DiscountRate = 0;

        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountRate = rate;
            DiscountCode = code;

        }

    }
}
