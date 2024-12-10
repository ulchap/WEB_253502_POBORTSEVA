using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;

namespace WEB_253502_POBORTSEVA.BlazorWASM.Services
{
    public class DataService : IDataService
    {
        private HttpClient _httpClient;
        private String _apiUri;
        private int _itemsPerPage;
        private JsonSerializerOptions _serializerOptions;
        private IAccessTokenProvider _accessTokenProvider;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient;
            _apiUri = configuration.GetValue<String>("ApiUri")!;
            _itemsPerPage = configuration.GetValue<int>("ItemsPerPage");

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _accessTokenProvider = accessTokenProvider;
        }

        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Product> Products { get; set; } = new List<Product>();

        public bool Success { get; set; }

        public string ErrorMessage { get; set; } = "";

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public Category SelectedCategory { get; set; }

        public event Action DataLoaded;

        public async Task GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_apiUri}Categories/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    Categories = (await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions)).Data;
                    DataLoaded?.Invoke();

                }
                catch (JsonException ex)
                {

                    Success = false;
                    ErrorMessage = $"Ошибка: {ex.Message}";
                    return;
                }
            }


            Success = false;
            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}";
            return;
        }
        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                var urlString = new StringBuilder($"{_apiUri}Products/category");

                if (categoryNormalizedName != null)
                {
                    urlString.Append($"/{categoryNormalizedName}");
                };

                var queryData = new List<string>
                {
                    $"pageNo={pageNo}",
                    $"pageSize={_itemsPerPage}"
                };

                if (queryData.Count > 0)
                {
                    urlString.Append("?" + string.Join("&", queryData));
                }


                //if (pageNo > 1)
                //{
                //    urlString.Append($"/page{pageNo}");
                //};
                //if (!_itemsPerPage.Equals(3))
                //{
                //    urlString.Append($"/pageSize{_itemsPerPage}");
                //}

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions)).Data!;
                        Products = result.Items;
                        TotalPages = result.TotalPages;
                        CurrentPage = result.CurrentPage;
                        DataLoaded?.Invoke();

                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                        return;
                    }
                }

                Success = false;
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}";
                return;
            }
        }

    }
}

