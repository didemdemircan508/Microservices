using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using IdentityModel.Client;

namespace FreeCourse.Web.Services.Interface
{
    public interface IIdentityService
    {

        Task<Response<bool>> SignIn(SigninInput signinInput);

        //tokenresponse otomatik olarak idenitymodelden   gelir
        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefsrehToken();
    }
}
