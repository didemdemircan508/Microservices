using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.PhotoStocks;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {

        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
           var response= await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;

        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
              return   null;

            }
            //fafafgsggs1899.jpeg path extensin dosyanın uzantısını alır
            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";

            using  var ms= new MemoryStream();//service gönderebilmem için byte çevrilmesi gerekmeketdir

            await  file.CopyToAsync(ms);//foto memorystreme kopyalandı.

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);

            var response = await _httpClient.PostAsync("photos", multipartContent);
            if(!response.IsSuccessStatusCode)
            {
                return null;

            }

            var viewModel = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return viewModel.Data;


        }
    }
}
