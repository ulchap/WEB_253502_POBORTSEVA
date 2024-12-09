using Microsoft.EntityFrameworkCore;
using WEB_253502_POBORTSEVA.API.Data;
using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.API.Services.ProductService;
using WEB_253502_POBORTSEVA.Domain.Entities;
using Humanizer.Localisation;
using Microsoft.Data.Sqlite;

namespace WEB_253502_POBORTSEVA.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var category1 = new Category { Id = 1, Name = "Category1", NormalizedName = "category1" };
            var category2 = new Category { Id = 2, Name = "Category1", NormalizedName = "category2" };

            context.Products.AddRange(
                new Product { Id = 1, Name = "Product1", Description = "Description1", Category = category1, Price = 100 }, 
                new Product { Id = 2, Name = "Product2", Description = "Description2", Category = category2, Price = 200 }, 
                new Product { Id = 3, Name = "Product3", Description = "Description3", Category = category1, Price = 300 }, 
                new Product { Id = 4, Name = "Product4", Description = "Description4", Category = category1, Price = 400 }, 
                new Product { Id = 5, Name = "Product5", Description = "Description5", Category = category2, Price = 500 });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null);
            var result = await service.GetProductListAsync(null);

            Assert.IsType<ResponseData<ListModel<Product>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.Products.First(), result.Data.Items[0]);
        }

        [Fact]
        public async Task ServiceCorrectlySelectsGivenPage()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null);
            var result = await service.GetProductListAsync(null, pageNo: 2);

            Assert.True(result.Successfull);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(2, result.Data.Items.Count); // 5 продуктов в базе данных, 3 на первой странице, 2 на второй
        }

        [Fact]
        public async Task ServiceCorrectlyFiltersByCategory()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null);
            var result = await service.GetProductListAsync("category1");

            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count); // 3 продукта в категории "category1"
            Assert.All(result.Data.Items, p => Assert.Equal("category1", p.Category.NormalizedName));
        }


        [Fact]
        public async Task ServiceDoesNotAllowPageSizeAboveMax()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null);
            var result = await service.GetProductListAsync(null, pageSize: 25);

            Assert.True(result.Successfull);
            Assert.True(result.Data.Items.Count <= 20);
        }

        [Fact]
        public async Task ServiceReturnsErrorIfPageNumberExceedsTotalPages()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null);
            var result = await service.GetProductListAsync(null, pageNo: 3);

            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }

    }
}
