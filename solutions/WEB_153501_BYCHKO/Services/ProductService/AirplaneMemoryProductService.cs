using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Services.EngineTypeCategoryService;

namespace WEB_153501_BYCHKO.Services.ProductService
{
    public class AirplaneMemoryProductService : IAirplaneProductService
    {
        List<Airplane> _airplanes;
        List<EngineTypeCategory> _engineTypes;
        IConfiguration _config;

        public AirplaneMemoryProductService([FromServices] IConfiguration config,
                                            IEngineTypeCategoryService categoryService)
        {
            _engineTypes = categoryService.GetCategoryListAsync().Result.Data;
            _config = config;
            SetupData();
        }

        // формирует данные предметной области
        private void SetupData()
        {
            _airplanes = new List<Airplane> 
            {
                new Airplane
                {
                    Id=1,
                    Name="Boeing 737-800",
                    Description="очень хороший самолет 10/10",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("turbofan"))!,
                    Price=125,
                    PhotoPath="Images/boeing737800.jpeg",
                },
                new Airplane
                {
                    Id=2,
                    Name="F-16",
                    Description="еще один очень хороший самолет 10/10",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("propfan"))!,
                    Price=1700,
                    PhotoPath="Images/f16.jpeg",
                },
                new Airplane
                {
                    Id=3,
                    Name="ИЛ-76",
                    Description="большй классный грузовичек, много повидал на своем веку 10/10",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("reciprocating"))!,
                    Price=25,
                    PhotoPath="Images/il76.jpeg",
                },
                new Airplane
                {
                    Id=4,
                    Name="Airbus A320",
                    Description="очень неплохой самолет 10/10",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("turbofan"))!,
                    Price=156,
                    PhotoPath="Images/airbusa320.jpeg",
                },
                 new Airplane{
                    Id =5,
                    Name = "Boeing 747",
                    Description = "икона стиля среди дальнемагистральных самолетов 100/10",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("turbofan"))!,
                    Price = 350,
                    PhotoPath = "Images/boeing747.jpeg",
                },
                new Airplane
                {
                    Id = 6,
                    Name = "Cessna 172",
                    Description = "простая рабочая лошадка, всем советую 10/10",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("reciprocating"))!,
                    Price = 75,
                    PhotoPath = "Images/cessna172.jpeg",
                },
                new Airplane
                {
                    Id = 7,
                    Name = "Antonov An-225",
                    Description = "огромная бандура, понятия не имею как она летает 10/10",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("propfan"))!,
                    Price = 500,
                    PhotoPath = "Images/antonov225.jpeg",
                },
                new Airplane
                {
                    Id = 8,
                    Name = "Beechcraft King Air",
                    Description = "идеально подходит для темных дел южноамереканских картелей",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("turboprop"))!,
                    Price = 250,
                    PhotoPath = "Images/kingair.jpeg",
                },


            };
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

        public Task<ResponseData<ListModel<Airplane>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // объект ответа
            var response = new ResponseData<ListModel<Airplane>>();
            // данный которые засунутся в объект ответа
            ListModel<Airplane> listModel = new ListModel<Airplane>();

            var pageSize = int.Parse(_config["ItemsPerPage"]!);

            // проверка на допуститмость номера страницы
            // 
            // иф такой хитрый, чтобы учесть плюс одну страницу, если она не полностью заполнена
            if (pageNo*pageSize - _airplanes.Count > pageSize)
            {
                throw new Exception("page number are greater then amount of pages");
            }

            // filteredAirplanes вынесен в отдельную переменную, только чтобы получить общее количество
            // самолетов соответствующих данной категории (надо для корректного отображения номеров страниц)
            var filteredAirplanes = _airplanes.
                Where(d => categoryNormalizedName == null ||
                    d.Category.NormalizedName.Equals(categoryNormalizedName)); // фильтр по категории
                
            listModel.Items = filteredAirplanes.Skip((pageNo - 1) * 3). // пропускаем элементы, которые не будут отображены
                Take(pageSize). // выбираем столько, сколько поместится на страницу
                ToList(); // конвертируем в список


            // округляем в большую сторону чтобы поместились все элементы
            var totalPages = Math.Ceiling((double)filteredAirplanes.Count() / (double)pageSize);
            listModel.TotalPages = (int)totalPages;
            listModel.CurrentPage = pageNo;

            // если нет самолетов соответствующих данной категории
            // сообщаем об ошибке
            if (listModel.Items.Count == 0)
            {
                response.Success = false;
                response.ErrorMessage = "can't find airplanes with such engine type";
            }

            // заносим данные в объект ответа
            response.Data = listModel;
            return Task.FromResult(response);
        }

        public Task UpdateProductAsync(int id, Airplane product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
