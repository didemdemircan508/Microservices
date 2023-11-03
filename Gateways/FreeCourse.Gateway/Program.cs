using FreeCourse.Gateway.DelegateHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();




//builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAddAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration.GetValue<string>("IdentityServerURL");
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;

});

builder.Services.AddOcelot();

//builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>();
var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

await app.UseOcelot();
//ocelot routlamayý kendi yapýyor o yüzden mapget ihtiyacýmýz yok

//app.MapGet("/", () => "Hello World!");

app.Run();
