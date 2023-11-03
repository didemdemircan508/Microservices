using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService,SharedIdentityService>();
builder.Services.AddScoped<IDiscountService,DiscountService>();

//sub da bir kullanýcý id bekliyorum
var requireAutHorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//addpermission write permission
//new AuthorizationPolicyBuilder().RequireClaim("scope", "discount_read");

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//jwt içerisndeki sub 'in name identifier olarak maplenmsinin enegllenmesi

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    options.Audience = "resource_discount";
    options.RequireHttpsMetadata = false;

});


// Add services to the container.
// addatuthorize all controller
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAutHorizePolicy));//baþarýlý kayýt yapmýþ bir kullanýcý bekliyorsam bu olamlý

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
