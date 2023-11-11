using FreeCourse.Shared.Services;
using FreeCourse.Web.Handler;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));


builder.Services.AddHttpContextAccessor();
builder.Services.AddAccessTokenManagement();//IClientCredentialTokenService nesne örneðini üretiyor
builder.Services.AddSingleton<PhotoHelper>();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

var serviceApiSettings=builder.Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
builder.Services.AddHttpClient<IClientCredentialTokenService,ClientCredentialTokenService>();


builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();
builder.Services.AddHttpClient<IIdentityService, IdentityService>();
//catalog servis kullanýcý doðrulamaya ihtiyac duymadýðý için ClientCredentialTokenHandler ekleiyoruz
builder.Services.AddHttpClient<ICatalogService,CatalogService>(opt=>
{
    opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
} ).AddHttpMessageHandler<ClientCredentialTokenHandler>();

//photostock servis kullanýcý doðrulamaya ihtiyac duymadýðý için ClientCredentialTokenHandler ekleiyoruz
builder.Services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
{
    opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
}).AddHttpMessageHandler<ClientCredentialTokenHandler>();




builder.Services.AddHttpClient<IUserService, UserService>(opt =>
{
    opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);

}).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();//Iuser service de client istek baþlattýðýnda ResourceOwnerPasswordTokenHandler kullanarak isteðe token ekle


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Auth/SignIn";
    options.ExpireTimeSpan = TimeSpan.FromDays(60);// refreshtoken ömrü 60 olduðu için burdada 60 verebiliriz
    options.SlidingExpiration = true; // 60 gün sürekli uzasýnmý?
    options.Cookie.Name= "DidemCookie";
    //ccokie oluþtuðu anda token claimsler bu cookie içerisinde bulunur
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
