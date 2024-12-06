using Microsoft.EntityFrameworkCore;
using WEB_253502_POBORTSEVA.API.Data;
using WEB_253502_POBORTSEVA.UI.Services.FileService;
using WEB_253502_POBORTSEVA.UI;
using WEB_253502_POBORTSEVA.UI.Services.CategoryService;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;
using WEB_253502_POBORTSEVA.UI.HelperClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WEB_253502_POBORTSEVA.UI.Services.Authentication;
using WEB_253502_POBORTSEVA.UI.Services.Authorization;
using WEB_253502_POBORTSEVA.UI.TagHelpers;
using WEB_253502_POBORTSEVA.UI.Services.CartService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PagerTagHelper>();

builder.Services.AddRazorPages();

//builder.RegisterCustomServices();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
    opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));

builder.Services.AddScoped(SessionCart.GetCart);

builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); // Customize scopes as needed 
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http 
    options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "admin",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.Run();
