using FreeCourse.Shared.Services;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            //token içerisinde kullanıcı id alıyoruz:_sharedIdentityService.GetUserId
            return View(await _catalogService.GetByCourseIdAsync(_sharedIdentityService.GetUserId));
        }
    }
}
