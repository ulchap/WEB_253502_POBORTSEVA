using Microsoft.AspNetCore.Mvc;
using Moq;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.UI.Controllers;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;
using WEB_253502_POBORTSEVA.UI.Services.CategoryService;
using NSubstitute;
using Microsoft.AspNetCore.Http;

namespace WEB_253502_POBORTSEVA.Tests
{
    public class ProductControllerTests
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _productService = Substitute.For<IProductService>();
            _categoryService = Substitute.For<ICategoryService>();
            _productController = new ProductController(_productService, _categoryService);
        }

        [Fact]
        public async Task Index_Returns404WhenCategoriesNotReceived()
        {
            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>> { Successfull = false });
            var result = await _productController.Index(null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Index_Returns404WhenProductsNotReceived()
        {
            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>> { Successfull = true });
            _productService.GetProductListAsync(It.IsAny<string?>()).Returns(new ResponseData<ListModel<Product>> { Successfull = false });
            var result = await _productController.Index(null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Index_SetsViewData_WithCategoriesAndCurrentCategory()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Name = "Category1", NormalizedName = "category1" },
                new Category { Name = "Category2", NormalizedName = "category2" }
            };

            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>> { Successfull = true, Data = categories });
            _productService.GetProductListAsync("category1", 1).Returns(new ResponseData<ListModel<Product>> { Successfull = true, Data = new ListModel<Product>() });

            var controllerContext = new ControllerContext();
            // Макет HttpContext
            var moqHttpContext = new Mock<HttpContext>(); 
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;

            _productController.ControllerContext = controllerContext;

            // Act
            var result = await _productController.Index("category1");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(categories, _productController.ViewData["categories"]);
            Assert.Equal("Category1", _productController.ViewData["currentCategory"]);
        }

        [Fact]
        public async Task Index_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new ListModel<Product>
            {
                Items = new List<Product> { new Product { Name = "Product1" }, new Product { Name = "Product2" } },
                TotalPages = 1
            };
            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>> { Successfull = true, Data = new List<Category>() });
            _productService.GetProductListAsync(null, 1).Returns(new ResponseData<ListModel<Product>> { Successfull = true, Data = products });

            var controllerContext = new ControllerContext();
            // Макет HttpContext
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;

            _productController.ControllerContext = controllerContext;

            // Act
            var result = await _productController.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(products, viewResult.Model);
        }
    }

}

