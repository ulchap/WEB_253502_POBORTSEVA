using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;

namespace WEB_253502_POBORTSEVA.UI.Services.ProductService
{
    public interface IProductService
    {
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        public Task<ResponseData<Product>> GetProductByIdAsync(int id);
        public Task<ResponseData<Product>> UpdateProductAsync(int id, Product product, IFormFile? formFile);
        public Task<ResponseData<string>> DeleteProductAsync(int id);
        public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile);

    }
}
