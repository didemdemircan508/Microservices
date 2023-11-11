using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http;

namespace FreeCourse.Web.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        //user olmadığı için gelen token cookie içerisine eklenmiyor ,elimizde cookie yok o yüzden memorye kaydetmem lazım
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;    //tokeni cache tutmak için 
        private readonly HttpClient _httpClient;


        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, 
            IOptions<ClientSettings> clientSettings,
            IClientAccessTokenCache clientAccessTokenCache,
            HttpClient httpClient)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }

        public async Task<string> GetToken()
        {
            // ilk cache bakıyoruz WebClientToken isminli token varmı?

            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken",null);

            if (currentToken != null)
            {
                //token varsa dön
                return currentToken.AccessToken;
            }
            //token yoksa

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }


            });

            //  disco üzerinde varolan tüm endpointler bize gelmeketedir.

            if (disco.IsError)
            {
                throw disco.Exception;
            }
            //client credential type kullnarak istek yapıyprum

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = disco.TokenEndpoint


            };

            var newToken=await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            if (newToken.IsError)
            {
                throw newToken.Exception;
            }

            //cache acces token kaydedlıyor
            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken,newToken.ExpiresIn,null);
            return newToken.AccessToken;




        }
    }
}
