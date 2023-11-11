using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;

        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {


            var list = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId);
            //token içerisinde kullanıcı id alıyoruz:_sharedIdentityService.GetUserId
            return View(list);
        }


        public async Task<IActionResult> Create()
        {
            //ilk olarak tüm kategorileri çekiyoruz
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();


        }

        [HttpPost]

        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            courseCreateInput.UserId = _sharedIdentityService.GetUserId;
            //ModelState["UserId"].ValidationState = ModelValidationState.Valid;
            //ModelState["Picture"].ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid)
            {
                return View();
            }




            var result = await _catalogService.CreateCourseAsync(courseCreateInput);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Kayıt İşlemi Başarısızdır");
                return View();

            }

            return RedirectToAction(nameof(Index));



        }

        [HttpGet]

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseIdAsync(id);
            var categories = await _catalogService.GetAllCategoryAsync();



            if (course == null)
            {
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Price = course.Price,
                Feature = course.Feature,/*new FeatureViewModel { Duration = course.Feature.Duration },*/
                CategoryId = course.CategoryId,
                Picture = course.Picture,
                Description = course.Description,
                UserId = course.UserId




            };
            return View(courseUpdateInput);



        }
        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);


            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _catalogService.UpdateCourseAsync(courseUpdateInput);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Güncelleme İşlemi Başarısızdır");
                return View();

            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string id)
        {
            var result = await _catalogService.DeleteCourseAsync(id);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Silme İşlemi Başarısızdır");
                return View();

            }
            return RedirectToAction(nameof(Index));


        }
    }
}
