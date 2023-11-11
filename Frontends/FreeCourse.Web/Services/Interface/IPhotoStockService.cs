using FreeCourse.Web.Models.PhotoStocks;

namespace FreeCourse.Web.Services.Interface
{
    public interface IPhotoStockService
    {

        Task<PhotoViewModel>  UploadPhoto(IFormFile file);
        Task<bool> DeletePhoto(string photoUrl);


    }
}
