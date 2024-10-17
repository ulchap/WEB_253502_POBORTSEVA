using Microsoft.EntityFrameworkCore;
using WEB_253502_POBORTSEVA.Domain.Entities;

namespace WEB_253502_POBORTSEVA.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();

            if (!context.Categories.Any())
            {

                await context.Categories.AddRangeAsync(
                    new Category { Name = "Смартфоны", NormalizedName = "smartphones" },
                    new Category { Name = "Ноутбуки", NormalizedName = "laptops" },
                    new Category { Name = "Телевизоры", NormalizedName = "tvs" },
                    new Category { Name = "Бытовая техника", NormalizedName = "home-appliances" },
                    new Category { Name = "Наушники", NormalizedName = "headphones" },
                    new Category { Name = "Аксессуары", NormalizedName = "accessories" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                //var baseUrl = app.Configuration["AppUrl"];
                string imageRoot = $"{app.Configuration["AppUrl"]}/Images";
                List<Category> _categories = await context.Categories.ToListAsync();

                await context.Products.AddRangeAsync(
                    new Product
                    {
                        Name = "Smartphone",
                        Description = "Latest model with advanced features",
                        Price = 999,
                        ImagePath = $"{imageRoot}/phone1.png",
                        Category = _categories.Find(c => c.NormalizedName.Equals("smartphones"))
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Laptop",
                        Description = "High performance laptop",
                        Price = 1499,
                        ImagePath = $"{imageRoot}/laptop1.png",
                        Category = _categories.Find(c => c.NormalizedName.Equals("laptops"))
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Наушники",
                        Description = "Быстрая зарядка и активное шумоподавление",
                        Price = 299,
                        ImagePath = $"{imageRoot}/headphones1.png",
                        Category = _categories.Find(c => c.NormalizedName.Equals("headphones"))
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Телевизор",
                        Description = "Улучшенная цветопередача и тонкие грани",
                        Price = 1199,
                        ImagePath = $"{imageRoot}/tv1.png",
                        Category = _categories.Find(c => c.NormalizedName.Equals("tvs"))
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Пылесос",
                        Description = "Обновлённый дизайн",
                        Price = 799,
                        ImagePath = $"{imageRoot}/vacuum1.png",
                        Category = _categories.Find(c => c.NormalizedName.Equals("home-appliances"))
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
