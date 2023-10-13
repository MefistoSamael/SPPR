using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Services.ProductService;

namespace WEB_153501_BYCHKO.Services.EngineTypeCategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;

        public ApiCategoryService(HttpClient httpClient,
                                  ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<ResponseData<List<EngineTypeCategory>>> GetCategoryListAsync()
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}EngineTypeCategories/");

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //#pragma warning disable CS8603 // Possible null reference return.
                    var content = response.Content;
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<EngineTypeCategory>>>();
                    //#pragma warning restore CS8603 // Possible null reference return.

                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<EngineTypeCategory>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<List<EngineTypeCategory>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }
    }
    
}
