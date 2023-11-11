using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

namespace FreeCourse.Web.Handler
{
   

    public class ResourceOwnerPasswordTokenHandler:DelegatingHandler
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor contextAccessor,
            IIdentityService identityService,
            ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _contextAccessor = contextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        //her istekte SendAsync methodu araya girip çalışcak
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //accesstoken cookieden okuyoruz
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //token aldıktan sonra tokenı headera ekliyoruz
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response=await base.SendAsync(request, cancellationToken);
            //dönen response 401 se refreshtoken alıcağız
            if(response.StatusCode==HttpStatusCode.Unauthorized) 
            {
                //refresh token ile tekrar accesstoken alıyoruz
                var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();
                if(tokenResponse != null)
                {
                    //aldığımız accesstokenla aynı istegi tekrra gönderiyoruz
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
            //refresh tokenda işe yaramzsa yani geçersizse kullanıcı login ekranına göndermemiz gerek,hata firlatıcağız

            if(response.StatusCode==HttpStatusCode.Unauthorized)
            {
                //hata fırlatılcak
                throw new UnAuthorizeException();

            }
            return response;
        }
    }
}
