using System.Text.Json;
using System.Text;
using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;

namespace WEB_253502_POBORTSEVA.UI.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration,
                                ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products");

            var response = await _httpClient.PostAsJsonAsync(
            uri,
            product,
            _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                .Content
                .ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);

                return data;
            }
            _logger
            .LogError($"-----> object not created. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Product>
            {
                Successfull = false,
                ErrorMessage = $"Объект не создан. Error: {response.StatusCode.ToString()}"
            };

        }

        public async Task<ResponseData<string>> DeleteProductAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products/" + $"{id}");
            var response = await _httpClient.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<string>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<string>
                    {
                        Successfull = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Объект не удален. Error: {response.StatusCode.ToString()}");
            return new ResponseData<string>
            {
                Successfull = false,
                ErrorMessage = $"Объект не удалён. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products/" + $"{id}");
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Product>
                    {
                        Successfull = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Product>
            {
                Successfull = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Products");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"/{categoryNormalizedName}");
            };
            // добавить номер страницы в маршрут
           // if (pageNo > 1)
           // {
                urlString.Append($"/{pageNo}");
            //};
            // добавить размер страницы в строку запроса
            //if (!_pageSize.Equals("3"))
            //{
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            // }
            //https://localhost:7002/api/Products/smartphones/1?pageSize=3
            //https://localhost:7002/api/Products/1?pageSize=3
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Product>>
                    {
                        Successfull = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Product>>
            {
                Successfull = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<Product>> UpdateProductAsync(int id, Product product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products/" + $"{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
                    if (formFile != null)
                        return data;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Product>
                    {
                        Successfull = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"Объект не обновлен. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Product>
            {
                Successfull = false,
                ErrorMessage = $"Объект не обновлен. Error: {response.StatusCode}"
            };
        }
    }
}
