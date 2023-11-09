using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        //user olmadığı için gelen token cookie içerisine eklenmiyor ,elimizde cookie yok o yüzden memorye kaydetmem lazım
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;    //tokeni cache tutmak için 


        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, 
            IOptions<ClientSettings> clientSettings,
            IClientAccessTokenCache clientAccessTokenCache)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
        }

        public Task<string> GetToken()
        {
            throw new NotImplementedException();
        }
    }
}
