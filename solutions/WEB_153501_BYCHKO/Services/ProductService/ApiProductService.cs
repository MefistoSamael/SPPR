using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
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
        private readonly HttpContext _httpContext;

        public ApiProductService(HttpClient httpClient,
                                 IConfiguration configuration,
                                 ILogger<ApiProductService> logger,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<ResponseData<Airplane>> CreateProductAsync(Airplane product, IFormFile? formFile)
        {
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}airplanes/");

            // полчуение токена
            var token = await _httpContext.GetTokenAsync("access_token");
            // установка токена в заголовки для авторизации в api
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            //отправка запроса к api
            var response = await _httpClient.PostAsJsonAsync(new Uri(urlString.ToString()), product);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var plane = await response.Content.ReadFromJsonAsync<Airplane>();

                    if (formFile != null)
                        await SaveImageAsync(plane!.Id, formFile);

                    return new ResponseData<Airplane> { Data = plane! };
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return  new ResponseData<Airplane>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
                return new ResponseData<Airplane>
                {
                    Success = false,
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
                };

            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}airplanes/{id}");

            // полчуение токена
            var token = await _httpContext.GetTokenAsync("access_token");
            // установка токена в заголовки для авторизации в api
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // отправить запрос к API
            var response = await _httpClient.DeleteAsync(
            new Uri(urlString.ToString()));

        }

        public async Task<ResponseData<Airplane>> GetProductByIdAsync(int id)
        {
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}airplanes/{id}");

            // полчуение токена
            var token = await _httpContext.GetTokenAsync("access_token");
            // установка токена в заголовки для авторизации в api
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            
            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    #pragma warning disable CS8603 // Possible null reference return.
                    var content = response.Content;
                    return await response.Content.ReadFromJsonAsync<ResponseData<Airplane>>()!;
                    #pragma warning restore CS8603 // Possible null reference return.

                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Airplane>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Airplane>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };

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
                urlString.Append($"category={categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno{pageNo}/");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize!.Equals("3"))
            {
                urlString.Append($"pagesize{_pageSize}/");
            }

            // полчуение токена
            var token = await _httpContext.GetTokenAsync("access_token");
            // установка токена в заголовки для авторизации в api
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    #pragma warning disable CS8603 // Possible null reference return.    
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Airplane>>>();
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


        public async Task UpdateProductAsync(int id, Airplane product, IFormFile? formFile)
        {
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}airplanes/{id}");

            // полчуение токена
            var token = await _httpContext.GetTokenAsync("access_token");
            // установка токена в заголовки для авторизации в api
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            //отправка запроса к api
            var response = await _httpClient.PutAsJsonAsync(new Uri(urlString.ToString()), product);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var plane = await response.Content.ReadFromJsonAsync<Airplane>();

                    if (formFile != null)
                        await SaveImageAsync(plane!.Id, formFile);

                    return;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri
            ($"{_httpClient.BaseAddress.AbsoluteUri}Airplanes/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent =
            new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            var answ = await _httpClient.SendAsync(request);
            if (!answ.IsSuccessStatusCode)
                throw new Exception("Couldnt save image. Probably wrong plane id");
        }
    }
}

