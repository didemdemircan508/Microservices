using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {

        private readonly ICatalogService _catalogService;

        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.Get());
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseIdAsync(courseId);
            if (course == null)
            {
                ModelState.AddModelError(string.Empty, "Kurs Bulunamadı");
                return RedirectToAction(nameof(Index));

            }
            var basketItem = new BasketItemViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                Price = course.Price,
                Quantity = 1


            };

            var result = await _basketService.AddBasketItem(basketItem);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Ekleme İşlemi Başarısızdır");
              

            }

            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            var result = await _basketService.RemoveBasketItem(courseId);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Silme İşlemi Başarısızdır");
              

            } 
            
            return RedirectToAction(nameof(Index));


        }



        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if(!ModelState.IsValid)
            {
                var listError = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
                TempData["discountError"] = listError[1];
                return RedirectToAction(nameof(Index));
            }
            var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);
            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> CancelApplyDiscount()
        { 
            await _basketService.CancelApplyDiscount();
            return RedirectToAction(nameof(Index));
        
        }
    }
}
