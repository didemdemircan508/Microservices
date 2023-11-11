using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interface;
using System.ComponentModel;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
       
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService,PhotoHelper photoHelper  )
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            //photo service ilk olarak fotoğrafı gönderiyoruz.
            var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                courseCreateInput.Picture = resultPhotoService.Url;
            
            }
            //servisten dönen img yolunu alıcağız
            //gönderilecek datayi json formata cevirip ,servise o şekilde göndermek için
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);
            return response.IsSuccessStatusCode;//bool dödüğümden dolayı direk IsSuccessStatusCode dönebilirz
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");
            if (!response.IsSuccessStatusCode)
            {
                return null;

            }
            var responseSucess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responseSucess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            //http:localhost:5000/services/catalog/courses
            var response = await _httpClient.GetAsync("courses");
            if(!response.IsSuccessStatusCode)
            {
                return null;

            }
            var responseSucess=await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSucess.Data.ForEach(x =>
            {
                x.StockPrictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);// adsdsfsffs.png gelen fotoyu hhtp/localhost:5102/photos/adsdsfsffs.png çevirme işlemi
            });

            return responseSucess.Data;


        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            //  /api/[controller]/GetAllByUserId/{userId}

            var response = await _httpClient.GetAsync($"courses/GetAllByUserId/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;

            }
            var responseSucess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSucess.Data.ForEach(x => 
            {
               x.StockPrictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });


            return responseSucess.Data;


        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;

            }
            var responseSucess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            responseSucess.Data.StockPrictureUrl = _photoHelper.GetPhotoStockUrl(responseSucess.Data.Picture);



            return responseSucess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
               await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhotoService.Url;

            }

            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
