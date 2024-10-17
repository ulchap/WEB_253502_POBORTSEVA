using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;

namespace WEB_253502_POBORTSEVA.API.Services.ProductService
{
    public interface IProductService
    {
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        public Task<ResponseData<Product>> GetProductByIdAsync(int id);
        public Task UpdateProductAsync(int id, Product product);
        public Task DeleteProductAsync(int id);
        public Task<ResponseData<Product>> CreateProductAsync(Product product);
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);

    }
}
