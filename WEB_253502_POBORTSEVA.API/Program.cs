using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WEB_253502_POBORTSEVA.API.Data;
using WEB_253502_POBORTSEVA.API.Services.CategoryService;
using WEB_253502_POBORTSEVA.API.Services.ProductService;
using WEB_253502_POBORTSEVA.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var authServer = builder.Configuration
                        .GetSection("AuthServer")
                        .Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    // Адрес метаданных конфигурации OpenID 
    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";

    // Authority сервера аутентификации 
    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";

    // Audience для токена JWT 
    o.Audience = "account";

    // Запретить HTTPS для использования локальной версии Keycloak 
    o.RequireHttpsMetadata = false;
});


builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

builder.Services.AddCors(options => { options.AddPolicy("AllowSpecificOrigin", 
    builder => {
    builder.WithOrigins("https://localhost:7198") //Blazor URL
           .AllowAnyHeader() 
           .AllowAnyMethod(); 
    }); 
});

var app = builder.Build();
await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=GetProducts}/{category?}");

app.Run();
