using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));


builder.Services.AddHttpContextAccessor();

var serviceApiSettings=builder.Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
builder.Services.AddHttpClient<IIdentityService, IdentityService>();
builder.Services.AddHttpClient<IUserService, UserService>(opt => {
    opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);

});

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
