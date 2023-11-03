using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services
{
    public class UserService : IUserService
    {

        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {

            //burada identity servera istek atıyurz
            //tokendelegatehandler yaparak istek atarken istediğin headırına cookiedan okuduğum tokeni göndericeğim bu sayde her istekte token almıyacağım 
            return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");




        }
    }
}
