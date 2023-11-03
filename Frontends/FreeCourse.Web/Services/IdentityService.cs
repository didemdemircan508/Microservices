using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace FreeCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            //identy model kütüphanesinden gelmektedşr
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

            //ccokieden refresh token alıyouurz

            var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint



            };

            var token=await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                return null;
            
            }

            var authenticationTokens = new List<AuthenticationToken>()
            {
                 new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                 new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                   new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}


            };

            //elimde zaten bir cokie var o yüden pricipla baştan oluşturmuyruz cookinin içerisindekii alıyoruz
            //cookie güncellleme işlemi yapıypruz
            var authenticationResult = await _contextAccessor.HttpContext.AuthenticateAsync();

            var properties =authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationResult.Principal, properties);

            return token;



        }

        public  async Task RevokeRefsrehToken()
        {
            //kullanıcı çıkış yapıldığında veritabından silmek lazım

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
            //refsreh tokeni iptal ediyoruz
            var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address=disco.RevocationEndpoint,
                Token=refreshToken,
                TokenTypeHint="refresh_token"

            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SigninInput signinInput)
        {
            //identy model kütüphanesinden gelmektedşr
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
            //PasswordTokenRequest sınıfı toekn almak için gönedrmemiz gereken parametreleri alan sınıf
            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
                Address = disco.TokenEndpoint

            };
            //toekn almak için aşağıdaki methıd

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            //userin bazı bilgilerine ulaşmak için discoya istek atabiliriz
            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);
            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            var authenticationProperties = new AuthenticationProperties();


            authenticationProperties.StoreTokens(new List<AuthenticationToken>() {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                 new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                   new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}



                });

            //remmeber tikliyse cookie duracak
            authenticationProperties.IsPersistent = signinInput.IsRemember;
            //kullanıcıyı login yapıyor ve cookie oluşuyor

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);





        }
    }
}
