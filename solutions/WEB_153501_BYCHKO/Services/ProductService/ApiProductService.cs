using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient,
                                 IConfiguration configuration,
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

        public Task<ResponseData<Airplane>> CreateProductAsync(Airplane product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Airplane>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Airplane>>> GetProductListAsync(
                                         string? categoryNormalizedName,
                                         int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}airplanes/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno{pageNo}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize!.Equals("3"))
            {
                urlString.Append(QueryString.Create("pagesize", _pageSize));
            }

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Airplane>>>(_serializerOptions);
#pragma warning restore CS8603 // Possible null reference return.

                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Airplane>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");
                     return new ResponseData<ListModel<Airplane>>
                     {
                         Success = false,
                         ErrorMessage = $"Данные не получены от сервера. Error: { response.StatusCode.ToString() }"
                     };
        }


        public Task UpdateProductAsync(int id, Airplane product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
