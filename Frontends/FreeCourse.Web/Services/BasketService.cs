using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {

        private readonly HttpClient _httpClient;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketService(HttpClient httpClient, ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<bool> AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            var basket = await Get();
            if (basket != null)
            {
                if (!basket.basketItems.Any(x => x.CourseId == basketItemViewModel.CourseId))
                {
                    basket.basketItems.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.basketItems.Add(basketItemViewModel);

            }

           return  await SaveOrUpdate(basket);
        }

        public Task<bool> ApplyDiscount(string discountCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelApplyDiscount()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete()
        {
            var response = await _httpClient.DeleteAsync("baskets");
            return response.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> Get()
        {


            var response = await _httpClient.GetAsync("baskets");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
            return basketViewModel.Data;


        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {

            var basket = await Get();
            if (basket == null)
            {
                return false;
            }
            var deleteBasketItem = basket.basketItems.FirstOrDefault(x => x.CourseId == courseId);
            if (deleteBasketItem == null)
            {
                return false;
            }

            var deleteResult = basket.basketItems.Remove(deleteBasketItem);
            if (!deleteResult)
            {
                return false;

            }
            if (!basket.basketItems.Any())
            {
                basket.DiscountCode = string.Empty;

            }
            return await SaveOrUpdate(basket);
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            //var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);

            //return response.IsSuccessStatusCode;

            if (basketViewModel.basketItems.Count == 0)
            {
                var response = await _httpClient.DeleteAsync("baskets");
                return response.IsSuccessStatusCode;
            }
            else
            {
                basketViewModel.UserId = _sharedIdentityService.GetUserId;
                if(basketViewModel.DiscountCode==null) basketViewModel.DiscountCode = string.Empty;
                if (basketViewModel.DiscountRate == null) basketViewModel.DiscountRate = 0;
                var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);
                return response.IsSuccessStatusCode;
            }



        }
    }
}
