using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.BlazorWasm.Services
{
    public class _dataService : IDataService
    {
        public List<EngineTypeCategory> Categories { get; set; } = new();
        public List<Airplane> ObjectsList { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly HttpClient _httpClient;
        private readonly int _pageSize;
        private readonly IAccessTokenProvider _tokenProvider;

        public event Action DataLoaded;

        public _dataService(HttpClient httpClient, IConfiguration configuration,
            IAccessTokenProvider iatp)
        {
            _httpClient = httpClient;
            _pageSize = Convert.ToInt32(configuration.GetSection("ItemsPerPage").Value);
            _tokenProvider = iatp;
        }

        public async Task GetCategoryListAsync()
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}EngineTypeCategories/");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token.Value);

                // отправить запрос к API
                var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        Categories = (await response.Content.ReadFromJsonAsync<ResponseData<List<EngineTypeCategory>>>()).Data;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        Success = true;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}";
                }
            }
        }

        public async Task GetProductByIdAsync(int id)
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}airplanes/{id}");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token.Value);

                // отправить запрос к API
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    
                    try
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        ObjectsList.Add((await response.Content.ReadFromJsonAsync<ResponseData<Airplane>>()).Data);
    #pragma warning restore CS8602 // Dereference of a possibly null reference.
                        Success = true;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}";
                }
                throw new NotImplementedException();
            } 
        }


        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
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
            if (!_pageSize!.Equals(3))
            {
                urlString.Append($"pagesize{_pageSize!}/");
            }

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token.Value);

                // отправить запрос к API
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Airplane>>>();
                        ObjectsList = responseData.Data.Items;
                        Success = true;
                        CurrentPage = responseData.Data.CurrentPage;
                        TotalPages = responseData.Data.TotalPages;
                        DataLoaded.Invoke();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                    catch (Exception ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}";
                }
            }
        }
    }
}
